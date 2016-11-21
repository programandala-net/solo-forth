#!/bin/sh

# Search the library of Solo Forth for the regex $1.

# 2016-04-17: First version.
# 2016-04-24: Add $2 for optional parameters.

ack $1 $2 -r ~/forth/solo_forth/src/lib/*.fsb
