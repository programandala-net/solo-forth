#!/bin/sh

# locate.sh

# This file is part of Solo Forth
# http://programandala.net/en.program.solo_forth.html

# XXX UNDER DEVELOPMENT

# --------------------------------------------------------------
# Description

# Search the library of Solo Forth for definitions of the word $1.

# --------------------------------------------------------------
# History

# 2016-03-23: Start.
# 2016-04-13: File header.

# ==============================================================

#vim -p -c ":call search(\"${1}\")" $(ack -l "$1" /usr/local/share/gforth/current/**/**.fs) 

# works:
#ack -l ": $1 " src/lib/*.fsb

# XXX FIXME --
#ack -l ':\|code $1 ' src/lib/*.fsb

# XXX FIXME --
# grep \
#   -l \
#   -e "\<: \+$1 " \
#   -e "\<code \+$1 " \
#   src/lib/*.fsb

grep \
  -e " 2constant \+$1 " \
  -e " 2constant \+$1$" \
  -e " 2variable \+$1 " \
  -e " 2variable \+$1$" \
  -e " cconstant \+$1 " \
  -e " cconstant \+$1$" \
  -e " constant \+$1 " \
  -e " constant \+$1$" \
  -e " cvariable \+$1 " \
  -e " cvariable \+$1$" \
  -e " defer \+$1 " \
  -e " defer \+$1$" \
  -e " value \+$1 " \
  -e " value \+$1$" \
  -e " variable \+$1 " \
  -e " variable \+$1$" \
  -e "^: \+$1 " \
  -e "^: \+$1$" \
  -e "^code \+$1 " \
  -e "^code \+$1$" \
  -e "^cvariable \+$1 " \
  -e "^cvariable \+$1$" \
  -e "^defer \+$1 " \
  -e "^defer \+$1$" \
  -e "^label \+$1 " \
  -e "^label \+$1$" \
  -e "^macro \+$1 " \
  -e "^macro \+$1$" \
  -e "^variable \+$1 " \
  -e "^variable \+$1$" \
  src/lib/*.fsb
