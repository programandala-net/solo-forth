#/bin/sh

# boot_with_fuse_for_debugging.sh

# This file is part of Solo Forth
# http://programandala.net/en.program.solo_forth.html

# Boot Solo Forth on the Fuse emulator with breakpoints

# 2015-08-16: Start.
# 2016-08-05: Update with directories.
# 2016-10-27: Update filename.
# 2017-04-28: Update filename.
# 2017-05-05: Update name and path.

# ==============================================================

# 2016-04-20: Debug the ROM calculator support

fuse-gtk \
  ./disks/gplusdos/disk_0_boot.mgt \
  --debugger-command "break w 0x692A"\
 &

# fuse-gtk \
#   disks/gplusdos/disk0.mgt \
#   --debugger-command "break 0x336C
#   break 0x338C
#   break 0x33A1
#   break 0x353B" \
#  &

# 2016-04-10: Debug the tape support
#
# fuse-gtk \
#   disks/gplusdos/disk0.mgt \
#   --debugger-command "break 0x04C2
#   break 0x053C
#   break 0x0991
#   break 0x0970
#   break 0x0984
#   break 0x833B" \
#  &

