#/bin/sh

# boot.debug.sh

# 2015-08-16

# Launch Fuse with the following breakpoints:

# 0x6355 next
# 0x635B next2

fuse-gtk \
  solo_forth_disk_1.mgt \
  --debugger-command "break 0x77EA" \
 &

# break 0x798E" &

#   --debugger-command "break 0x635B" \
    # --debugger-command "break 0x8266
  #--debugger-command "break 0x6557 HL==0x65E8
  # break 0x8270
  # break 0x8275" \
