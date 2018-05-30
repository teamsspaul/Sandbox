import matplotlib
import matplotlib.pyplot as plt
import numpy as np

# Data for plotting
GateWidth = [18,20,22,24,26,30,34,40,50]
GateWidth = np.array(GateWidth)
Uncertainty = np.zeros((len(GateWidth),1))
for i in range(0,len(GateWidth)):
    Uncertainty[i,0] = np.exp(-GateWidth[i])+0.00000000001*GateWidth[i]**2

def MatPrint(Mat,rows,cols):
    s = "";
    for i in range(0,cols):
      for j in range(0,rows):
        s += "%.2f" % Mat[i,j] + " ";
      s += "\n";
    return s;

print(GateWidth)
print(Uncertainty)

Order = 9
#Fit to a polynomial
PF = np.polyfit(GateWidth,Uncertainty,Order)
Y = np.polyval(PF,GateWidth)

#Manual Fit of a polynomial
Cols = Order
Rows = len(GateWidth)
X = np.zeros((Rows,Cols))
for row in range(0,Rows):
    for col in range(0,Cols):
        X[row,col]=GateWidth[row]**col
print(MatPrint(X,Rows,Cols))
XT = np.zeros((Cols,Rows))
for row in range(0,Rows):
    for col in range(0,Cols):
        XT[col,row]=X[row,col]
print(MatPrint(XT,Cols,Rows))

a = np.matmul(np.matmul(np.linalg.inv(np.matmul(XT,X)),XT),Uncertainty)
aRev = np.zeros((len(GateWidth),1))
for i in range(0,len(GateWidth)):
    aRev[i] = a[len(GateWidth)-i-1]

fig, ax = plt.subplots()
ax.plot(GateWidth, Uncertainty,'b')
ax.plot(GateWidth, Y,'k')
ax.plot(GateWidth, np.polyval(aRev,GateWidth),'g')

ax.set(xlabel='Gate Width', ylabel='Percent Uncertainty')
ax.grid()

fig.savefig("test.png")
plt.show()
