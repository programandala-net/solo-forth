#!/bin/sh

# boot_with_fuse.sh

# This file is part of Solo Forth
# http://programandala.net/en.program.solo_forth.html

# Boot Solo Forth on the Fuse emulator

# 2015-06: Start
# 2016-08-05: Update with directory.
# 2016-10-27: Update filename.
# 2017-04-28: Update filename.
# 2017-05-05: Update name and path.

# ==============================================================

fuse-sdl \
  --speed 100 \
	--machine 128 \
	--graphics-filter 3x \
	--no-divide \
	--plusd \
  --plusddisk ./disks/gplusdos/disk_0_boot.mgt \
	$* \
	&
