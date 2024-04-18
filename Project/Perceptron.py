import sys;
import random;
import numpy;
import matplotlib.pyplot as plt;


class Perceptron:
    def __init__(self,dimension,learnrate):
        self.dimension=dimension
        self.inputs=[]
        self.weights= []
        self.output=float("NaN")
        self.learnrate = learnrate
        self.error=float("NaN")
        for i in range(dimension+1):
            self.weights.append(numpy.random.rand())
        
    
    def Train(self,pointlist,expected,cycles):
        points = self.AddBiasInput(pointlist)
        for c in range(cycles):
            for idx in range(len(points)):
                self.inputs = points[idx]
                self.output = self.EvalFunc()
                self.error = expected[idx] - self.output
                #print("Epoch {0},pointIDX {1},output {2},error {3},point {4},expected {5} ".format(c+1,idx,self.output,self.error,self.inputs,expected[idx]))
                self.RecalcWeights()
                self.SaveWeigths("Weights_{0}.txt".format(c+1))
            print("Epoch {0} finished with weights {1}".format(c+1,self.weights))
    
    def Test(self,pointlist):
        points = self.AddBiasInput(pointlist)
        outputs=[]
        for idx in range(len(points)):
            self.inputs = points[idx]
            outputs.append(self.EvalFunc())
        return outputs


    def EvalFunc(self):
        suma = numpy.dot(self.inputs,self.weights)
        #print("Point {0} weights {1} suma {2}".format(self.inputs,self.weights,suma))
        return numpy.sign(suma)
        
    def RecalcWeights(self):
        for idx in range(len(self.weights)):
            #print("{0} = {1} + {2}*{3}*{4}".format(self.weights[idx] + self.learnrate*self.error*self.output,self.weights[idx],self.learnrate,self.error,self.output))
            self.weights[idx] += self.learnrate*self.error*self.inputs[idx]
    
    def AddBiasInput(self,pointlist):
        colu = [1]*len(pointlist)
        newpoints=list(list(x) for x in numpy.insert(pointlist,0,colu,axis=1))
        return newpoints
    def SaveWeigths(self,filename):
        f = open(filename,"w")
        f.write(";".join((str(x) for x in self.weights)))
        f.close();
            


def GetPointsFromFile(filename,separator,dimension):
    pointlist=[]
    f = open(filename, "r")
    for line in f:
        pointlist.append(list(float(x) for x in (line.split(separator)[0:dimension])))
    f.close()
    print("Succesfully loaded {0} points".format(len(pointlist)))
    return pointlist

def GeneratePoints(dimension,pointcount,mini,maxi):
    pointlist = list(list(x) for x in numpy.round(numpy.random.rand(pointcount,dimension),1)*(maxi-mini)+mini)
    return pointlist
    
def GetExpectedValuesLinear(points,k,q):
    vals=numpy.subtract(numpy.array(points)[:,1],numpy.add(numpy.multiply(numpy.array(points)[:,0],k) ,q))
    expected = list(numpy.sign(vals))
    for idx in range(len(vals)):
        print("EXP: Point {0} value {1} exp {2}".format(points[idx],vals[idx],expected[idx]))
    print("---")
    return expected

def DrawPoints(points,k,q,mini,maxi,results):
    x = numpy.arange(-100,100,1)
    linefunc=k*x+q
    underpoints=[]
    abovepoints=[]
    onpoints=[]
    for p in range(len(points)):
        if(results[p] > 0):
            abovepoints.append(points[p])
            print("Showing point {0} above line".format(points[p]))
        elif(results[p]<0):
            underpoints.append(points[p])
            print("Showing point {0} under line".format(points[p]))
        else:
            onpoints.append(points[p])
            print("Showing point {0} on line".format(points[p]))
    plt.xlim([mini-1,maxi+1])
    plt.ylim([mini-1,maxi+1])
    print(len(underpoints))
    print(len(abovepoints))
    print(len(onpoints))
    
    if(len(underpoints)):
        plt.plot(numpy.array(underpoints)[:,0],numpy.array(underpoints)[:,1],"bo")
    if(len(abovepoints)):
        plt.plot(numpy.array(abovepoints)[:,0],numpy.array(abovepoints)[:,1],"go")
    if(len(onpoints)):
        plt.plot(numpy.array(onpoints)[:,0],numpy.array(onpoints)[:,1],"ro")
    
    plt.plot(x,linefunc,"-r")
    plt.grid()
    plt.show()

def main():
    dimension = 2
    learnrate = 0.1
    pointcount = 1000
    learncyclecount=60
    testpointcount = 30
    k=3
    q=2
    mini = -10
    maxi= 10
    perc = Perceptron(dimension,learnrate)
    trainset = GeneratePoints(dimension,pointcount,mini,maxi)
    #trainset=GetPointsFromFile("Trainset.txt",";",dimension)
    testset = GeneratePoints(dimension,testpointcount,mini,maxi)
    expectedresults=GetExpectedValuesLinear(trainset,k,q)
    testexpect = GetExpectedValuesLinear(testset,k,q)
    perc.Train(trainset,expectedresults,learncyclecount)
    results=perc.Test(testset)
    for idx in range(len(results)):
        print("Point {0} has result {1} ({2})".format(testset[idx],results[idx],testexpect[idx]))
    

    
    if(dimension == 2):
        DrawPoints(testset,k,q,mini,maxi,results)
    
if __name__ == "__main__":
    main()
