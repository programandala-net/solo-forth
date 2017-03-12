  \ exception.codes.trdos.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702220020
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ The Forth system error codes (range -4095..-256) used for
  \ TR-DOS (range -1012..-1000).

( TR-DOS error codes #-1000..#-1012 )

  \ TR-DOS Error codes and messages.
  \
  \ Note: Error #-1000 is not an error, but it's kept in order
  \ to preserve the range of the original actual errors of
  \ TR-DOS (1..12). That's also why the undocumented error
  \ #-1009 (not mentioned in the Beta 128 Disk manual) has been
  \ added.

#-1000 \ TR-DOS: No errors
#-1001 \ TR-DOS: No files
#-1002 \ TR-DOS: File exists
#-1003 \ TR-DOS: No space
#-1004 \ TR-DOS: Directory full
#-1005 \ TR-DOS: Record number overflow
#-1006 \ TR-DOS: No disk
#-1007 \ TR-DOS: Disk errors
#-1008 \ TR-DOS: Syntax errors
#-1009 \ TR-DOS: Undefined error
#-1010 \ TR-DOS: Stream already opened
#-1011 \ TR-DOS: Not disk file
#-1012 \ TR-DOS: Stream not open

  \ ===========================================================
  \ Change log

  \ 2016-08-04: Start.
  \
  \ 2017-02-05: Fix typo in error description.

  \ vim: filetype=soloforth
