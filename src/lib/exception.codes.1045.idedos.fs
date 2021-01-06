  \ exception.codes.idedos.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202101062038.
  \ See change log at the end of the file.

  \ ===========================================================
  \ Description

  \ The Forth system error codes (range -4095..-256) used for
  \ IDEDOS (range -1066..-1056).

  \ XXX REMARK: This file is deprecated. Its messages were
  \ added to <exception.codes.1000.plus3dos.fs>, and are also
  \ part of <exception.codes.1000.nextzxos.fs>.

( IDEDOS error codes #-1045..#-1059 )

#-1045 \ IDEDOS: (Unused error)
#-1046 \ IDEDOS: (Unused error)
#-1047 \ IDEDOS: (Unused error)
#-1048 \ IDEDOS: (Unused error)
#-1049 \ IDEDOS: (Unused error)
#-1050 \ IDEDOS: (Unused error)
#-1051 \ IDEDOS: (Unused error)
#-1052 \ IDEDOS: (Unused error)
#-1053 \ IDEDOS: (Unused error)
#-1054 \ IDEDOS: (Unused error)
#-1055 \ IDEDOS: (Unused error)
#-1056 \ IDEDOS: Invalid partition
#-1057 \ IDEDOS: Partition already exists
#-1058 \ IDEDOS: Not implemented
#-1059 \ IDEDOS: Partition open

( IDEDOS error codes #-1060..#-1066 )

#-1060 \ IDEDOS: Out of partition handles
#-1061 \ IDEDOS: Not a swap partition
#-1062 \ IDEDOS: Drive already mapped
#-1063 \ IDEDOS: Out of XDPBs
#-1064 \ IDEDOS: No swap partition available
#-1065 \ IDEDOS: Invalid device
#-1066 \ IDEDOS: 8-bit data transfer

  \ ===========================================================
  \ Change log

  \ 2016-05-18: Modify format of messages.
  \
  \ 2016-08-04: Add unused errors #-1045..#-1055, because
  \ IDEDOS is used always with +3DOS.
  \
  \ 2021-01-06: Deprecate this file, excluding it in
  \ <Makefile>. Copy the errors to
  \ <exception.codes.1000.plus3dos.fs>.

  \ vim: filetype=soloforth
