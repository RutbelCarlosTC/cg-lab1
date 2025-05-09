import numpy as np
import matplotlib.pyplot as plt
from matplotlib.patches import Circle

def campo_electrico(q, r0, x, y):
    """Calcula el campo eléctrico E=(Ex, Ey) de una carga q en r0"""
    den = np.hypot(x - r0[0], y - r0[1])**3
    return q * (x - r0[0]) / den, q * (y - r0[1]) / den

def plot_lineas_de_campo(q1, q2):
  r1 = np.array([-1, 0])  
  r2 = np.array([1, 0])  

  #Crear la malla de puntos donde se calculará el campo eléctrico
  nx, ny = 64, 64
  x = np.linspace(-2, 2, nx)
  y = np.linspace(-2, 2, ny)
  X, Y = np.meshgrid(x, y)


  # Inicializar los componentes del campo eléctrico
  Ex, Ey = np.zeros((ny, nx)), np.zeros((ny, nx))

  # Calcular el campo eléctrico debido a cada carga y sumarlos
  ex1, ey1 = campo_electrico(q1, r1, x=X, y=Y)
  Ex += ex1
  Ey += ey1

  ex2, ey2 = campo_electrico(q2, r2, x=X, y=Y)
  Ex += ex2
  Ey += ey2

  # Configurar la gráfica
  fig = plt.figure()
  ax = fig.add_subplot(111)

  #dibujar las lineas
  color = 2 * np.log(np.hypot(Ex, Ey))
  ax.streamplot(x, y, Ex, Ey, color=color, linewidth=1, cmap=plt.cm.inferno,
                density=2, arrowstyle='->', arrowsize=1.5)

  # Dibujar las cargas como círculos
  charge_colors = {True: '#aa0000', False: '#0000aa'}  # Mapeo de colores para cargas
  for q, pos in [(q1, r1), (q2, r2)]:
      ax.add_artist(Circle(pos, 0.05, color=charge_colors[q > 0]))

  # Configurar ejes y mostrar gráfica
  ax.set_xlabel('$x$')
  ax.set_ylabel('$y$')
  ax.set_xlim(-2, 2)
  ax.set_ylim(-2, 2)
  ax.set_aspect('equal')
  plt.show()


q1 = int(input("Ingrese carga del cuerpo 1 (C): "))
q2 = int(input("Ingrese carga del cuerpo 2 (C): "))

plot_lineas_de_campo(q1,q2)