#!/bin/bash

# fb2mgt.sh

# This file is part of Solo Forth
# http://programandala.net/en.program.solo_forth.html

# --------------------------------------------------------------
# Description

# This program converts a Forth source file blocks format to an
# MGT disk image.
#
# This is an alternative to fsb-mgt.sh (part of fsb, see
# http://programandala.net/en.program.fsb.html), and fsb2-mgt.sh
# (part of fsb2, see
# http://programandala.net/en.program.fsb2.html), which check
# the blocks of the FSB before creating the MGT.
#
# fb2mgt.sh does not check the blocks of the source file.  It is
# intended to be used in combination with <fs2fba.sh>. The FBA
# format is a Forth blocks file, but only the first line is a
# block header.
#
# Usage
#
#   fb2mgt.sh filename.fb

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

# 2016-05-13: Extracted from fsb2-mgt.sh, a converter included
# in fsb2 (http://programandala.net/en.program.fsb2.html).
#
# 2016-06-01: Fix description.

# --------------------------------------------------------------
# Error checking

if [ "$#" -ne 1 ] ; then
  echo "Convert a Forth source file from .fb to .mgt"
  echo 'Usage:'
  echo "  ${0##*/} sourcefile.fb"
  exit 1
fi

if [ ! -e "$1"  ] ; then
  echo "Error: <$1> does not exist"
  exit 1
fi

if [ ! -f "$1"  ] ; then
  echo "Error: <$1> is not a regular file"
  exit 1
fi

if [ ! -r "$1"  ] ; then
  echo "Error: <$1> can not be read"
  exit 1
fi

if [ ! -s "$1"  ] ; then
  echo "Error: <$1> is empty"
  exit 1
fi

# --------------------------------------------------------------
# Main

# Get the filenames:

basefilename=${1%.*}
blocksfile=$basefilename.fb
mgtfile=$basefilename.mgt

# Get the size of the file:
du_size=$(du -sk $blocksfile)

# Extract the size from the left of the string:
file_size=${du_size%%[^0-9]*}

echo "File size=($file_size)"
#echo "$blocksfile is $file_size Kib"

if [ $file_size -gt "800" ]
then
  echo "Error:"
  echo "The size of $blocksfile is $file_size KiB."
  echo "The maximum capacity of an MGT disk image is 800 KiB."
  exit 64
fi

# Do it:

dd if=$blocksfile of=$mgtfile bs=819200 cbs=819200 conv=block,sync 2>> /dev/null

# Remove the temporary file:

rm -f $blocksfile

# vim:tw=64:ts=2:sts=2:et:
