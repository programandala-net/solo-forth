#/bin/sh

# boot.debug.sh

# 2015-08-16
# 2016-04-10 updated to debug the tape support

# Launch Fuse with the following breakpoints:


fuse-gtk \
  solo_forth_disk_1.mgt \
  --debugger-command "break 0x04C2
  break 0x053C
  break 0x0991
  break 0x0970
  break 0x0984
  break 0x833B" \
 &

# break 0x798E" &

#   --debugger-command "break 0x635B" \
    # --debugger-command "break 0x8266
  #--debugger-command "break 0x6557 HL==0x65E8
  # break 0x8270
  # break 0x8275" \
