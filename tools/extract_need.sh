#!/bin/bash

# extract_need.sh

# This file is part of Solo Forth
# http://programandala.net/en.program.solo_forth.html

# XXX UNDER DEVELOPMENT

# --------------------------------------------------------------
# Description

# Search Solo Forth sources for `need`.

# --------------------------------------------------------------
# History

# 2016-03-23: Start.
# 2016-04-13: File header.

# ==============================================================

# Extract a list of `need X` from the sources,
# where `X` is a needed word:
need_requirements=$(grep --only-matching --no-filename \
  -e " need \+\S\+" \
  -e "^need \+\S\+" \
  src/lib/*.fsb )

#echo $need_requirements
#read

#echo $requirements | \
#  sed -e 's/\(^need \)\|\( need \)\|\( need$\)/\n/g' | sort | uniq > _meta/tmp/needed_words.txt

# Remove the `need` words:
need_words_1=$(echo $need_requirements | sed -e 's/\(^need \)\|\( need \)\|\( need$\)/ /g' )

echo "<<< $need_words_1 >>>" ; read

# XXX TODO --
# needed_requirements=$(grep --only-matching --no-filename \
#   -e "s\" .\+\" \+needed " \
#   -e "s\" .\+\" \+needed$" \
#   src/lib/*.fsb )

# Remove the `needed` words:
# XXX TODO --
#need_words_2=$(echo $needed_requirements | sed -e 's/\(^s" \+\(\S\+\)" needed \)\|\( need \)\|\( need$\)/\n/g' )

requirements=$(echo $need_words_1 | sort | uniq)

echo $requirements | \
  > _meta/tmp/requirements.txt
