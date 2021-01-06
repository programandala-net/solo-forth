  \ exception.codes.nextzxos.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202101061945.
  \ See change log at the end of the file.

  \ ===========================================================
  \ Description

  \ The Forth system error codes (range -4095..-256) used for
  \ NextZXOS.

( NextZXOS error codes #-1000..#-1014 )

#-1000 \ NextZXOS: Drive not ready
#-1001 \ NextZXOS: Disk is write protected
#-1002 \ NextZXOS: Seek fail
#-1003 \ NextZXOS: CRC data error
#-1004 \ NextZXOS: No data
#-1005 \ NextZXOS: Missing address mark
#-1006 \ NextZXOS: Unrecognised disk format
#-1007 \ NextZXOS: Unknown disk error
#-1008 \ NextZXOS: Disk changed whilst NextZXOS was using it
#-1009 \ NextZXOS: Unsuitable media for drive
#-1010 \ NextZXOS: (Unused error)
#-1011 \ NextZXOS: (Unused error)
#-1012 \ NextZXOS: (Unused error)
#-1013 \ NextZXOS: (Unused error)
#-1014 \ NextZXOS: (Unused error)

( NextZXOS errors #-1015..#-1029 )

#-1015 \ NextZXOS: (Unused error)
#-1016 \ NextZXOS: (Unused error)
#-1017 \ NextZXOS: (Unused error)
#-1018 \ NextZXOS: (Unused error)
#-1019 \ NextZXOS: (Unused error)
#-1020 \ NextZXOS: Bad filename
#-1021 \ NextZXOS: Bad parameter
#-1022 \ NextZXOS: Drive not found
#-1023 \ NextZXOS: File not found
#-1024 \ NextZXOS: File already exists
#-1025 \ NextZXOS: End of file
#-1026 \ NextZXOS: Disk full
#-1027 \ NextZXOS: Directory full
#-1028 \ NextZXOS: Read-only file
#-1029 \ NextZXOS: File number not open (or with wrong access)

( NextZXOS error codes #-1030..#-1043 )

#-1030 \ NextZXOS: Access denied (file is in use already)
#-1031 \ NextZXOS: Cannot rename between drives
#-1032 \ NextZXOS: Extent missing (which should be there)
#-1033 \ NextZXOS: Uncached (software error)
#-1034 \ NextZXOS: File too big
#-1035 \ NextZXOS: Disk not bootable (boot sector)
#-1036 \ NextZXOS: Drive in use
#-1037 \ NextZXOS: (Unused error)
#-1038 \ NextZXOS: (Unused error)
#-1039 \ NextZXOS: (Unused error)
#-1040 \ NextZXOS: (Unused error)
#-1041 \ NextZXOS: (Unused error)
#-1042 \ NextZXOS: (Unused error)
#-1043 \ NextZXOS: (Unused error)

( NextZXOS error codes #-1044..#-1058 )

#-1044 \ NextZXOS: (Unused error)
#-1045 \ NextZXOS: (Unused error)
#-1046 \ NextZXOS: (Unused error)
#-1047 \ NextZXOS: (Unused error)
#-1048 \ NextZXOS: (Unused error)
#-1049 \ NextZXOS: (Unused error)
#-1050 \ NextZXOS: (Unused error)
#-1051 \ NextZXOS: (Unused error)
#-1052 \ NextZXOS: (Unused error)
#-1053 \ NextZXOS: (Unused error)
#-1054 \ NextZXOS: (Unused error)
#-1055 \ NextZXOS: (Unused error)
#-1056 \ NextZXOS: Invalid partition
#-1057 \ NextZXOS: Partition already exists
#-1058 \ NextZXOS: Not implemented

( NextZXOS error codes #-1059..#-1073 )

#-1059 \ NextZXOS: Partition open
#-1060 \ NextZXOS: Out of handles
#-1061 \ NextZXOS: Not a swap partition
#-1062 \ NextZXOS: Drive already mapped
#-1063 \ NextZXOS: No XDPB
#-1064 \ NextZXOS: No suitable swap partition
#-1065 \ NextZXOS: Invalid device
#-1066 \ NextZXOS: (Unused error)
#-1067 \ NextZXOS: Command phase error
#-1068 \ NextZXOS: Data phase error
#-1069 \ NextZXOS: Not a directory
#-1070 \ NextZXOS: (Unused error)
#-1071 \ NextZXOS: (Unused error)
#-1072 \ NextZXOS: (Unused error)
#-1073 \ NextZXOS: (Unused error)

( NextZXOS error codes #-1074..#-1074 )

#-1074 \ NextZXOS: File is fragmented, use .DEFRAG

  \ ===========================================================
  \ Change log

  \ 2021-01-06: Start. Copy the content of
  \ <exception.codes.1000.plus3dos.fs>. Modify and complete the
  \ errors after "NextZXOS and esxDOS APIs (Updated 20 Jan
  \ 2020)" (page 24).

  \ vim: filetype=soloforth
