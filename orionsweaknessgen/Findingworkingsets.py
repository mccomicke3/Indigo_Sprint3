import random
import matplotlib.pyplot as plt
import numpy as np

weaknessset8 = [[2, 2, 2, 2, 2, 3],
    [2, 2, 2, 2, 3, 2],
    [2, 2, 2, 3, 2, 2],
    [2, 2, 3, 2, 2, 2],
    [2, 3, 2, 2, 2, 2],
    [3, 2, 2, 2, 2, 2],
    [2, 2, 2, 2, 4],
    [2, 2, 2, 4, 2],
    [2, 2, 4, 2, 2],
    [2, 4, 2, 2, 2],
    [4, 2, 2, 2, 2],
    [2, 2, 2, 3, 3],
    [2, 2, 3, 2, 3],
    [2, 2, 3, 3, 2],
    [2, 3, 2, 2, 3],
    [2, 3, 2, 3, 2],
    [2, 3, 3, 2, 2],
    [3, 2, 2, 2, 3],
    [3, 2, 2, 3, 2],
    [3, 2, 3, 2, 2],
    [3, 3, 2, 2, 2],
    [2, 2, 3, 4],
    [2, 2, 4, 3],
    [2, 3, 2, 4],
    [2, 3, 4, 2],
    [2, 4, 2, 3],
    [2, 4, 3, 2],
    [3, 2, 2, 4],
    [3, 2, 4, 2],
    [3, 4, 2, 2],
    [4, 2, 2, 3],
    [4, 2, 3, 2],
    [4, 3, 2, 2],
    [2, 3, 3, 3],
    [3, 2, 3, 3],
    [3, 3, 2, 3],
    [3, 3, 3, 2],
    [2, 4, 4],
    [4, 2, 4],
    [4, 4, 2],
    [3, 3, 4],
    [3, 4, 3],
    [4, 3, 3]]

weaknessset6 = [[2, 2, 2, 2, 2],
[3, 4],
[4, 3],
[3, 3, 2],
[3, 2, 3],
[2, 3, 3],
[2, 2, 4],
[2, 4, 2],
[4, 2, 2]]












'''-----------------------------------------------------------------------------
This algorithm will find the possible lengths of weaknesses which will

A weakness must follow the following criteria

- consist of w moves between (2, 4)
- there are 4 possible moves and in this code will be represented by integers
  0, 1, 2, 3
- each concequtive weakness begins with the last entry of the first weakness
- each weakness must be unique
- each combo must meld together and fit into the ultimate combo in 8 moves.


Because of these restrictions there are a certain number of possible lengths of
weaknesses which will work and will not work. 

The total number of moves in the ultimate combo must be 8. This means that the 
total combination of the length of the weaknesses can be larger than 8 because
of the way that the moves overlap. for example.
Weaknesses:
002
200
021
12


Total length of the weaknesses = 3+3+3+2 = 11
Number of weaknesses = 4
The final combo will be 00200212 
lenweaknesses - numberofweaknesses - 1 = 8

This combo works, but in order for this to be the case the total length of
the weaknesses subtracted by the amount of overlap (the number of weaknesses -1)
must equal 8. Rearranging the terms we can see that any valid weakness list must
follow this relationship. 

lenweaknesses - numberofweaknesses = 9

for example, this is a combo list which would not work. 
032
231
122
233


length of weaknesses = 3+3+3+3 = 12
numberofweaknesses = 4
12 - 4 = 9, 8 != 9
032312233
because this is not equal to 9 the combo will not work. 

Whether or not the combo is workable seems to entirely depend on the length of
the weaknesses, there must be a certain number of sequences of weakness lengths
which are possible.

any order of 5 2-length weaknesses and 1 3-length weakness
any order of 3 3-length weaknesses and 1 2-length weakness

These are the known possible lengths of viable sets of weaknesses
I am fairly sure that this is an exhaustive list, but you can call the 
brutetest function to attempt any set of lengths but I think this is it. 

2222222
222223
22224
22233
2234
2333
244
334

and all possible permutaions of these sets.

if we randomly take from the possible permutaions of htese sets, because there
are a lot more permuations of these sets possible for the values with many
entries, then the average case may not be great, so we might want to exclude 
some of these in the final build, but this is the clay we have to work with. 

-----------------------------------------------------------------------------'''

def brutetest(targetchar): #usage Test: 223332 --> either pass or fail 
    userinput = input('Test: ')
    while userinput != '': #exit loop with null entry
        charsum = 0
        for char in userinput:
            charsum = charsum + int(char)
        totalchar = len(userinput)
        print(f'{charsum} - {totalchar} -1 = {charsum - (totalchar - 1)}')
        if charsum - (totalchar - 1)== targetchar:
            print('Pass')
        else:
            print('Fail')

        userinput = input('Test: ')

#brutetest(6)

def permutation(lst): 
  
    # If lst is empty then there are no permutations 
    if len(lst) == 0: 
        return [] 
  
    # If there is only one element in lst then, only 
    # one permuatation is possible 
    if len(lst) == 1: 
        return [lst] 
  
    # Find the permutations for lst if there are 
    # more than 1 characters 
  
    l = [] # empty list that will store current permutation 
  
    # Iterate the input(lst) and calculate the permutation 
    for i in range(len(lst)): 
       m = lst[i] 
  
       # Extract lst[i] or m from the list.  remLst is 
       # remaining list 
       remLst = lst[:i] + lst[i+1:] 
  
       # Generating all permutations where m is first 
       # element 
       for p in permutation(remLst): 
           l.append([m] + p) 
    return l 
'''-----------------------------------------------------------------------------
Acting under the assumption that this list is indeed exhaustive we can use it to
produce a randomly generating algorithm which will produce a set of weaknesses
of lengths according to the sets. 
-----------------------------------------------------------------------------'''
def findpermutations(outfilename, combinationlist):

    roughlist = []
    outfile = open(outfilename, 'w')
    for combination in combinationlist:
        permutationrough = permutation(combination)
        for permute in permutationrough:
            roughlist.append(permute)
    finallist=[]
    for permute in roughlist:
        if permute not in finallist:
            finallist.append(permute)
            outfile.write(str(permute)+'\n')
    print (finallist)
    outfile.close()
#findpermutations('possibleweaknesses.txt','w',)
combinationlist =   [[2,2,2,2,2],
                    [3,4],
                    [3,3,2],
                    [2,2,4]]
#findpermutations('possibleweaknesses6.txt', combinationlist)

def weaknessgenerator(weaknessset): 
#  [2, 2, 2, 4, 2]
    choosenlengths = weaknessset[random.randint(0,len(weaknessset)-1)]
    finalweaknesses = []
    generatedweakness = ''
    previousmove = random.randint(0,3)

    for weaknesslen in choosenlengths:
        generatedweakness = ''
        generatedweakness = generatedweakness + str(previousmove)

        generatedweakness = uniqueweakness(generatedweakness, finalweaknesses, weaknesslen)
        previousmove = generatedweakness[-1]
        finalweaknesses.append(generatedweakness)

    return(finalweaknesses)



def uniqueweakness(incompleteweakness, finalweaknesses, weaknesslen):
    for move in range(weaknesslen-1):
        incompleteweakness = incompleteweakness + str(random.randint(0,3))

    if incompleteweakness in finalweaknesses:
        #print('recursion')
        incompleteweakness = uniqueweakness(incompleteweakness[0], finalweaknesses, weaknesslen)

    return incompleteweakness

def weaknessgeneratortest(n):
    for test in range (n):
        print(weaknessgenerator())


def foundweakness(foundweaknesses, weaknesslist, userinput):
    for weakness in weaknesslist:
        if weakness in userinput: 
            if weakness not in foundweaknesses: 
                foundweaknesses.append(weakness)
    return foundweaknesses


#print(finalcombo(['21', '112', '203', '33', '31'],'21120331'))
def finalcombo(weaknesslist, userinput):
    
    for weakness in weaknesslist:
        if weakness not in userinput:
            return False
    return True
        






# a simple little test function to get a feel for
#how a game might play on a personal level. 

def testgame(): 
    weaknesslist = weaknessgenerator()
    turntimer = 0
    foundweaknesses = []

    while True:
        #print(weaknesslist)
        for weakness in weaknesslist:
            print(len(weakness), end=' ')
        turntimer += 1
        print(f'Turn: {turntimer}\nFound Weaknesses: {foundweaknesses}')
        userinput = input('Input moves: ')
        if finalcombo(weaknesslist, userinput) == True: 
            break
        
        foundweaknesses = foundweakness(foundweaknesses, weaknesslist, userinput)
    print('You Win, Combo Complete')

#testgame()



#the idea for this function is to quantitatively test the difficulty 
#of the game by randomly entering values and seeing how many turns
#it would take on average to resolve a game. 


def difficultytest(weaknessset, lenfullcombo):
    turnsuntilwin = []


    for combination in weaknessset:
        combinationtrialturns = []
        for test in range (30):
            trialsuccess = False
            trialturns = 0
            trialweaknesses = weaknessgenerator()
            while trialsuccess == False:
                trialturns += 1
                userimput = ''

                for digit in range (lenfullcombo):
                    userimput = userimput + str(random.randint(0,3))

                if finalcombo(trialweaknesses, userimput):
                    trialsuccess = True
                if trialturns > 1000:
                    trialsuccess = True
                    print('exceeded 1000 trials')

            combinationtrialturns.append(trialturns)
        turnsuntilwin.append(combinationtrialturns)
    
    print(turnsuntilwin)
    finaldata = []
    for dataset in turnsuntilwin:
        finaldata.append(sum(dataset)/len(dataset))
    finaldataarray = np.array(finaldata)
    x = np.arange(0, len(weaknessset))
    plt.bar(x, finaldataarray)
    plt.show()
    #holy fucking shit these odds are terrible. 
    #it may actually be 1 in 4^8 on average. 
    #we may have to scrap this approach entirely
    # I mean i dont know, the person will have
    #more information than that, they'll know the 
    #length of the combos and that one will always
    #lead into the next. 

#difficultytest(weaknessset6)

def equallists(list1, list2):
    for item in list1:
        if item not in list2:
            return False
    for item2 in list2:
        if item2 not in list1:
            return False
    return True

#alright this algorithm just measures success as having
#found all of the combo peices, but I feel like
#this may still be low balling the number of 
#turns. idfk its fucking late dude. 

def weaknessfindtest(weaknessset, lenfullcombo):
    turnsuntilwin = []

    for combination in weaknessset:
        combinationtrialturns = []
        for test in range (300):
            trialsuccess = False
            trialturns = 0
            trialweaknesses = weaknessgenerator(weaknessset)
            trialfoundweaknesses = []
            while trialsuccess == False:
                trialturns += 1
                userimput = ''
                
                for digit in range (lenfullcombo):
                    userimput = userimput + str(random.randint(0,3))

                trialfoundweaknesses = foundweakness( trialfoundweaknesses, trialweaknesses, userimput)
                print(userimput)
                if equallists(trialfoundweaknesses, trialweaknesses):
                    trialsuccess = True

                if trialturns > 1000:
                    trialsuccess = True
                    print('exceeded 1000 trials')

            combinationtrialturns.append(trialturns)
        turnsuntilwin.append(combinationtrialturns)
    
    print(turnsuntilwin)
    finaldata = []
    for dataset in turnsuntilwin:
        finaldata.append(sum(dataset)/len(dataset))
    finaldataarray = np.array(finaldata)
    x = np.arange(0, len(weaknessset))
    plt.bar(x, finaldataarray)
    plt.title(f'Number of average trials needed to find all weaknesses '
              f'length of combo: {lenfullcombo}')
    plt.show()

#alright this is looking a lot less bad, doable even. 
#still seems pretty boring to input the same thing 30 times
#on average
#weaknessfindtest(weaknessset6, 6)








