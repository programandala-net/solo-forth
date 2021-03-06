  \ exception.codes.plus3dos.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702220020
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ The Forth system error codes (range -4095..-256) used for
  \ +3DOS.

( +3DOS error codes #-1000..#-1014 )

#-1000 \ +3DOS: Drive not ready
#-1001 \ +3DOS: Disk is write protected
#-1002 \ +3DOS: Seek fail
#-1003 \ +3DOS: CRC data error
#-1004 \ +3DOS: No data
#-1005 \ +3DOS: Missing address mark
#-1006 \ +3DOS: Unrecognised disk format
#-1007 \ +3DOS: Unknown disk error
#-1008 \ +3DOS: Disk changed whilst +3DOS was using it
#-1009 \ +3DOS: Unsuitable media for drive
#-1010 \ +3DOS: (Unused error)
#-1011 \ +3DOS: (Unused error)
#-1012 \ +3DOS: (Unused error)
#-1013 \ +3DOS: (Unused error)
#-1014 \ +3DOS: (Unused error)

( +3DOS errors #-1015..#-1029 )

#-1015 \ +3DOS: (Unused error)
#-1016 \ +3DOS: (Unused error)
#-1017 \ +3DOS: (Unused error)
#-1018 \ +3DOS: (Unused error)
#-1019 \ +3DOS: (Unused error)
#-1020 \ +3DOS: Bad filename
#-1021 \ +3DOS: Bad parameter
#-1022 \ +3DOS: Drive not found
#-1023 \ +3DOS: File not found
#-1024 \ +3DOS: File already exists
#-1025 \ +3DOS: End of file
#-1026 \ +3DOS: Disk full
#-1027 \ +3DOS: Directory full
#-1028 \ +3DOS: Read-only file
#-1029 \ +3DOS: File number not open (or with wrong access)

( +3DOS error codes #-1030..#-1036 )

#-1030 \ +3DOS: Access denied (file is in use already)
#-1031 \ +3DOS: Cannot rename between drives
#-1032 \ +3DOS: Extent missing (which should be there)
#-1033 \ +3DOS: Uncached (software error)
#-1034 \ +3DOS: File too big (trying to read/write past 8 MB)
#-1035 \ +3DOS: Disk not bootable (boot sector not acceptable)
#-1036 \ +3DOS: Drive in use (remap/remove with files open)

  \ ===========================================================
  \ Change log

  \ 2016-05-18: Modify format of messages.
  \
  \ 2016-08-04: Add range #-1000..#-1014 to simplify the
  \ calculation, whatever the DOS implementation.

  \ vim: filetype=soloforth
