import time
import struct
from numpy import random


def str2ar(str1):
    ar=str1.split(";")
    res=[]
    for i in range(1,int(float(ar[0]))+1):
        t=float(ar[i])
        res.append(t)
    return res

def ar2str(ar):
    s=str(len(ar))+";"
    for x in ar:
        s=s+str(x)+";"    
    return s


f = open(r'\\.\pipe\NamedPipedtest', 'r+b', 0)

while True:
    temp = (random.rand(random.randint(5)+2)-0.5)*2
    sendAr=[round(item, 3) for item in temp]
    s=ar2str(sendAr)
     
    f.write(struct.pack('I', len(s)) + s.encode('ascii'))
    f.seek(0)
    print('-----------------------------------------')
    print('Send Array: ',end='')
    print(sendAr)
    
    
    n = struct.unpack('I', f.read(4))[0] 
    s = f.read(n).decode('ascii')
    f.seek(0)
    recAr=str2ar(s)
    print('Recieved Array: ',end='')
    print(recAr)
    print('-----------------------------------------')

    time.sleep(2)
