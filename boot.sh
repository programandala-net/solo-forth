#!/bin/sh
# solo_forth.sh
# 2015-06

# boot.sh

fuse \
  --speed 100 \
	--machine 128 \
	--no-divide \
	--plusd \
  --plusddisk ./solo_forth_disk_1.mgt \
  --plusddisk ./solo_forth_disk_2.mgt \
	$* \
	&

