import matplotlib
import matplotlib.pyplot as plt
import numpy as np

# Data for plotting
with open('Input175.txt') as f:
    content = f.readlines()

GateWidth = np.zeros((len(content)))
Uncertainty = np.zeros((len(content)))
for i in range(0,len(content)):
    line = content[i].split("\n")[0]
    GateWidth[i] = float(line.split(",")[0])
    Uncertainty[i] = float(line.split(",")[1])

GateWidth,Uncertainty = (list(t) for t in zip(*sorted(zip(GateWidth,Uncertainty))))
#print(GateWidth)
print(Uncertainty)
    
def MatPrint(Mat,rows,cols):
    s = "";
    for i in range(0,cols):
      for j in range(0,rows):
        s += "%.2f" % Mat[j,i] + " ";
      s += "\n";
    return s;
def MatPrintE(Mat,rows,cols):
    s = "";
    for i in range(0,rows):
      for j in range(0,cols):
        s += "%.2e" % Mat[i,j] + " ";
      s += "\n";
    return s;

Order = 9
#Fit to a polynomial
PF = np.polyfit(GateWidth,Uncertainty,Order)
print(PF)
Y = np.polyval(PF,GateWidth)

#Manual Fit of a polynomial
Cols = Order+1
Rows = len(GateWidth)
X = np.zeros((Rows,Cols))
for row in range(0,Rows):
    for col in range(0,Cols):
        X[row,col]=GateWidth[row]**col
#print(MatPrintE(X,Rows,Cols))
XT = np.zeros((Cols,Rows))
for row in range(0,Rows):
    for col in range(0,Cols):
        XT[col,row]=X[row,col]
#print(MatPrintE(XT,Cols,Rows))

a = np.matmul(np.matmul(np.linalg.inv(np.matmul(XT,X)),XT),Uncertainty)
print(a)
#quit()
#aRev = np.zeros((len(GateWidth),1))
#for i in range(0,len(GateWidth)):
#    aRev[i] = a[len(GateWidth)-i-1]

fig, ax = plt.subplots()
ax.plot(GateWidth, Uncertainty,'b')
ax.plot(GateWidth, Y,'k')
#ax.plot(GateWidth, np.polyval(aRev,GateWidth),'g')

ax.set(xlabel='Gate Width', ylabel='Percent Uncertainty')
ax.grid()

fig.savefig("test.png")
plt.show()
