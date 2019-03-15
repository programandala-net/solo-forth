#!/bin/sh
# fs2fba.sh

# This file is part of Solo Forth
# http://programandala.net/en.program.solo_forth.html

# Last modified 201903151335.
# See change log at the end of the file.

# ==============================================================
# Author

# Marcos Cruz (programandala.net), 2016, 2019.

# ==============================================================
# License

# You may do whatever you want with this work, so long as you
# retain every copyright, credit and authorship notice, and
# this license.  There is no warranty.

# ==============================================================
# Description

# This program converts a Forth source file from text to blocks,
# compacting it by removing empty lines and FSB metacomments.  This
# makes it possible to write a Forth program in ordinary text format,
# without the constraints imposed by the intermediate format FSB, and
# then integrate it into the Solo Forth library disk, in order to load
# it as a whole with `load-app name`.
#
# The only requisite of the source is that its first actual line of
# code (the first line that is not empty or an FSB metacomment) must
# be a block header, with a name as identifier (a word surrounded by
# spaces), which will be used with `load-app`.
#
# This program is intended to be copied to the projects written in
# Solo Forth and called from Makefile.
#
# See the Makefile of projects _Black Flag_
# (http://programandala.net/en.program.black_flag.html) and _Nuclear
# Waste Invaders_
# (http://programandala.net/en.program.nuclear_waste_invaders.html)
# for an usage example.

# ==============================================================
# Requisites

# - vim
# - dd

# ==============================================================

if [ "$#" = "0" ]; then
  echo 'fs2fba'
  echo 'Usage:'
  echo "  ${0##*/} <filename> [<filename>] ..."
  exit 1
fi

for file in $*
do

  base_filename=${file%.*}
  output_file="$base_filename.fba"
  tmp_file=$(mktemp --suffix=.tmp $base_filename-XXX)

  # Remove empty lines and meta line comments (backslash line
  # comments preceded by at least one space):
  vim -e \
    -c "%s@^\\(\\s\\+\\\\.*\\)\\?\\n@@e" \
    -c "saveas! $tmp_file" \
    -c "quit!" \
    $file ; \

  # Create the blocks file:
  dd if=$tmp_file of=$output_file bs=1024 cbs=64 conv=block,sync

  rm -f $tmp_file

done

exit 0

# ==============================================================
# Change log

# 2016-05-13: Start, based on <fs2fb.sh>, by the same author.
#
# 2016-06-01: Fix description.
#
# 2016-12-19: Fix problem with grep.
#
# 2019-03-15: Fix and rewrite. grep was causing problems again: This
# step removed empty lines, but somehow it removed also all code lines
# containing non-ASCII characters!: `grep "^.\+" $file`.  This
# alternative made no difference: `grep --invert-match "^$" $file`.
# Finally grep has been replaced by Vim.  Improve documentation.
