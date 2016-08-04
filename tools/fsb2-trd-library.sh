#!/bin/bash

# fsb2-trd-library.sh

# This file is part of fsb2
# http://programandala.net/en.program.fsb2.html

# ##############################################################
# Author and license

# Copyright (C) 2016 Marcos Cruz (programandala.net)

# You may do whatever you want with this work, so long as you
# retain the copyright notice(s) and this license in all
# redistributed copies and derived works. There is no warranty.

# ##############################################################
# Description

# This program converts a Forth source file from the FSB format to a
# ZX Spectrum phony TRD disk image (suitable for TR-DOS), The disk
# image will contain the source file directly on the sectors, without
# file system, to be directly accessed by a Forth system.  This is the
# format used by the library disk of Solo Forth
# (http://programandala.net/en.program.solo_forth.html).
#
# XXX TODO -- Add track 0 (16 sectors, 4 KiB) at the start, created by
# <make_trd_track_0.fs>, which is part of Solo Forth.

# ##############################################################
# Requirements

# fsb2:
#   <http://programandala.net/en.program.fsb2.html>

# ##############################################################
# Usage

#   fsb2-trd-library.sh filename.fsb

# ##############################################################
# History

# 2016-08-03: Start. Adapt from fsb2-trd.sh

# ##############################################################
# Error checking

if [ "$#" -ne 1 ] ; then
  echo "Convert a Forth source file from .fsb to .trd"
  echo 'Usage:'
  echo "  ${0##*/} sourcefile.fsb"
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

# ##############################################################
# Main

fsb2 $1

# Get the filenames:

basefilename=${1%.*}
blocksfile=$basefilename.fsb.fb
trdfile=$basefilename.trd

# Get the size of the file:
du_size=$(du -sk $blocksfile)

# Extract the size from the left of the string:
file_size=${du_size%%[^0-9]*}

#echo "File size=($file_size)"
#echo "$blocksfile is $file_size Kib"

if [ $file_size -gt "636" ]
then
  echo "Error:"
  echo "The size of $blocksfile is $file_size KiB."
  echo "The maximum capacity of a formated TRD disk image is 636 KiB."
  exit 64
fi

# Do it:

dd if=$blocksfile of=$trdfile bs=651264 cbs=651264 conv=block,sync 2>> /dev/null



# Remove the temporary file:

rm -f $blocksfile

# vim:tw=64:ts=2:sts=2:et:

