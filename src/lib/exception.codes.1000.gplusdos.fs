  \ exception.codes.gplusdos.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702220020

  \ -----------------------------------------------------------
  \ Description

  \ The Forth system error codes (range -4095..-256) used for
  \ G+DOS.

  \ -----------------------------------------------------------
  \ Latest changes

  \ 2016-05-18: Modify format of messages.

( G+DOS error codes #-1000..#-1014 )

  \ G+DOS Error codes and messages.
  \ Some of them are useless for this implementation.

#-1000 \ G+DOS: Nonsense in G+DOS
#-1001 \ G+DOS: Nonsense in GNOS
#-1002 \ G+DOS: Statement end error
#-1003 \ G+DOS: Break requested
#-1004 \ G+DOS: Sector error
#-1005 \ G+DOS: Format data lost
#-1006 \ G+DOS: Check disk in drive
#-1007 \ G+DOS: No +SYS file
#-1008 \ G+DOS: Invalid file name
#-1009 \ G+DOS: Invalid station
#-1010 \ G+DOS: Invalid device
#-1011 \ G+DOS: Variable not found
#-1012 \ G+DOS: Verify failed
#-1013 \ G+DOS: Wrong file type
#-1014 \ G+DOS: Merge error

( G+DOS error codes #-1015..#-1029 )

  \ G+DOS Error codes and messages.
  \ Some of them are useless for this implementation.

#-1015 \ G+DOS: Code error
#-1016 \ G+DOS: Pupil set
#-1017 \ G+DOS: Invalid code
#-1018 \ G+DOS: Reading a write file
#-1019 \ G+DOS: Writing a read file
#-1020 \ G+DOS: O.K. G+DOS
#-1021 \ G+DOS: Network off
#-1022 \ G+DOS: Wrong drive
#-1023 \ G+DOS: Disk write protected
#-1024 \ G+DOS: Not enough space on disk
#-1025 \ G+DOS: Directory full
#-1026 \ G+DOS: File not found
#-1027 \ G+DOS: End of file
#-1028 \ G+DOS: File name used
#-1029 \ G+DOS: No G+DOS loaded

( G+DOS error codes #-1030..#-1031 )

#-1030 \ G+DOS: STREAM used
#-1031 \ G+DOS: CHANNEL used

  \ vim: filetype=soloforth
