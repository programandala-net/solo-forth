#/bin/sh

# 2015-08-16

# Launch fuse with the following breakpoints:

# 0x6369 next_2_end
# 0x6A6C do_colon_end

#fuse-gtk --debugger-command "br 0x6369
#  br 0x6A6C"  solo_forth_disk_1.mgt  &

# 0x7404 cold_start

#  fuse-gtk --debugger-command "br 0x7474"  solo_forth_disk_1.mgt  &

# 0x5E00 cold_entry

  fuse-gtk --debugger-command "br 0x5E00"  solo_forth_disk_1.mgt  &
