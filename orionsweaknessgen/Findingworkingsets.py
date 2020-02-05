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



def brutetest(): #usage Test: 223332 --> either pass or fail 
    userinput = input('Test: ')
    while userinput != '': #exit loop with null entry
        charsum = 0
        for char in userinput:
            charsum += int(char)
        totalchar = len(userinput)
        print(f'{charsum} - {totalchar} -1 = {charsum - (totalchar - 1)}')
        if charsum - (totalchar - 1)== 8:
            print('Pass')
        else:
            print('Fail')

        userinput = input('Test: ')

#brutetest()
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
def findpermutations():
    combinationlist =   [[2,2,2,2,2,2,2,],
                        [2,2,2,2,2,3],
                        [2,2,2,2,4],
                        [2,2,2,3,3],
                        [2,2,3,4],
                        [2,3,3,3],
                        [2,4,4],
                        [3,3,4]]

    roughlist = []
    outfile = open('possibleweaknesses.txt','w')
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


def weaknessgenerator(): # this isnt finished yet but I'll work on it. 
    weaknessset = [[2, 2, 2, 2, 2, 3],
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




#this is a very ineffiecent method of finding all these permutations but I 
#care not! for it is 3 am and i am very tired. also it works so fuck it.

#findpermutations()

