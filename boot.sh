#!/bin/sh

# boot.sh

# This file is part of Solo Forth
# http://programandala.net/en.program.solo_forth.html

# Boot Solo Forth on the Fuse emulator

# 2015-06: Start
# 2016-08-05: Update with directory.
# 2016-10-27: Update filename.

# ==============================================================

fuse-sdl \
  --speed 100 \
	--machine 128 \
	--no-divide \
	--plusd \
  --plusddisk ./disks/gplusdos/disk0.mgt \
	$* \
	&
