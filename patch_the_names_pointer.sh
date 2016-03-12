#!/bin/bash

# patch_the_names_pointer.sh

# Get the value of `np` from the assembly list produced by the
# assembler and patch the binary with it.

# 2015-08-17

# Unfinished. It seems it won't be needed.

list_file="solo_forth.list.txt"
binary_file="forth.bin"

data_line=$(grep "data:........ np$" $list_file)

echo $data_line

value=${data_line##.+:0000}

echo $value

