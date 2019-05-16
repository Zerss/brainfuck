#!/usr/bin/python3

import sys

if len( sys.argv ) != 2:
    sys.stderr.write( "Usage: {0} <code> or {0} <path>\n".format( sys.argv[0] ) )
    sys.exit(1)

def content( path ):
    with open( path, 'r' ) as file:
        return file.read()

def isPath( val ):
    common_characters = ". "
    brainfuck_characters = "><+-[].,"
    for i in range( 0, len( val ) ):
        if val[i] in common_characters: continue
        if val[i] in brainfuck_characters: return False
        else: return True
    return True

code = content( sys.argv[1] ) if isPath( sys.argv[1] ) else sys.argv[1]
index = 0
memory = [0]
mem_index = 0

loop_indexes = []

while index < len( code ):
    ch = code[index]
    if ch == '>':
        mem_index += 1
        if mem_index == len( memory ): memory.append(0)
    elif ch == '<':
        mem_index -= 1
        if mem_index < 0:
            sys.stderr.write( "Error: Cursor cannot point at a negative index" )
            sys.exit(1)
    elif ch == '+':
        memory[mem_index] += 1
        memory[mem_index] %= 256
    elif ch == '-':
        memory[mem_index] -= 1
        memory[mem_index] %= 256
    elif ch == '.':
        sys.stdout.write( chr( memory[mem_index] ) )
    elif ch == ',':
        memory[mem_index] = ord( input() )
    elif ch == '[':
        if memory[mem_index] != 0: loop_indexes.append( index )
        else:
            count = 0
            while code[index] != ']' or count > 1:
                if code[index] == '[': count += 1
                elif code[index] == ']': count -= 1
                index += 1
    elif ch == ']':
        loop_index = loop_indexes.pop()
        if memory[mem_index] != 0: index = loop_index - 1
    index += 1

print()
