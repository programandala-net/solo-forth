#/bin/sh

# 2015-08-16

# Launch fuse with the following breakpoints:

# 0x6369 next_2_end
# 0x6A6C do_colon_end

#fuse-gtk --debugger-command "br 0x6369
#  br 0x6A6C"  solo_forth_disk_1.mgt  &

#  fuse-gtk --debugger-command "br 0x7474"  solo_forth_disk_1.mgt  &

# 0x5E00 cold_entry
#    --debugger-command "br 0x5E00"  \
#   --debugger-command "br 0x79B4" \

  fuse-gtk \
   --debugger-command "break 0x8266
  break 0x8270
  break 0x8275" \
    solo_forth_disk_1.mgt  &

    # --debugger-command "commands 1
  # br 0x6672
  # br 0x6681
  # br 0x66A8
  # br 0x669D
  # end" \
