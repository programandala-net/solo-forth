#!/bin/sh
# fs2fba.sh

# This file is part of Solo Forth
# http://programandala.net/en.program.solo_forth.html

# --------------------------------------------------------------
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

# --------------------------------------------------------------
# Author

# Marcos Cruz (programandala.net), 2016.

# --------------------------------------------------------------
# License

# You may do whatever you want with this work, so long as you
# retain every copyright, credit and authorship notice, and
# this license.  There is no warranty.

# --------------------------------------------------------------
# History

# 2016-05-13: Start, based on <fs2fb.sh>, by the same author.
# 2016-06-01: Fix description.
# 2016-12-19: Fix problem with grep.

# --------------------------------------------------------------

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

  # Remove empty lines and metacomments:

  # XXX OLD -- 2016-12-19: this expression removes lines that contain
  # Latin1 non-ASCII chars, but it works fine with UTF-8 texts, why?:
  #
  # grep "^.\+$" $file | \

  # XXX NEW -- 2016-12-19: the problem is solved when `$` is removed,
  # why?:
  grep "^.\+" $file | \
  grep --invert-match "^\s\+\\\\\s" | \
  grep --invert-match "^\s\+\\\\$" \
  > $tmp_file

  # ################################################
  # XXX TMP -- debugging test:

  # # XXX FIXME -- this removes lines that contain latin1 non-ASCII
  # # chars, why?!:
  # grep "^.\+$" $file > src/XXX-step-1-grep.tmp
  # # XXX FIXME -- ack creates an empty file, why?
  # ack "^.\+$" $file > src/XXX-step-1-ack.tmp
  # # XXX FIXME -- no difference:
  # grep --extended-regexp "^.+$" $file > src/XXX-step-grep-1E.tmp
  # # XXX REMARK -- solution:
  # grep "^.\+" $file > src/XXX-step-1-grep-1D.tmp

  # ################################################

  # Create the blocks file:
  dd if=$tmp_file of=$output_file bs=1024 cbs=64 conv=block,sync

  rm -f $tmp_file

done

exit 0
