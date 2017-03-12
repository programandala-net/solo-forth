  \ exception.codes.os.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ XXX UNDER DEVELOPMENT

  \ Last modified: 201702220020
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ The Forth system error codes (range -4095..-256) used for
  \ ZX Spectrum OS (range -1154..-1128).

( OS error codes #-1128..#-1142 )

  \ XXX TODO -- it seems OS error codes (there are 28 of them)
  \ may be returned by G+DOS.  they are detected and converted
  \ by `ior>error`.
  \
  \ XXX TODO -- Move this to the G+DOS file and integrate the
  \ calculation. Study in the G+DOS documentation, which OS
  \ errors can be actually issued by DOS operations, not by DOS
  \ commands entered in the BASIC CLI.

#-1128 \ OS: OK
#-1129 \ OS: NEXT without FOR
#-1130 \ OS: Variable not found
#-1131 \ OS: Subscript wrong
#-1132 \ OS: Out of memory
#-1133 \ OS: Out of screen
#-1134 \ OS: Number too big
#-1135 \ OS: RETURN without GO SUB
#-1136 \ OS: End of file
#-1137 \ OS: STOP statement
#-1138 \ OS: Invalid argument
#-1139 \ OS: Integer out of range
#-1140 \ OS: Nonsense in BASIC
#-1141 \ OS: BREAK - CONT repeats
#-1142 \ OS: Out of DATA

( OS error codes #-1143..#-1154 )

#-1143 \ OS: Invalid file name
#-1144 \ OS: No room for line
#-1145 \ OS: STOP in INPUT
#-1146 \ OS: FOR without NEXT
#-1147 \ OS: Invalid I/O device
#-1148 \ OS: Invalid colour
#-1149 \ OS: BREAK into program
#-1150 \ OS: RAMTOP no good
#-1151 \ OS: Statement lost
#-1151 \ OS: Invalid stream
#-1152 \ OS: FN without DEF
#-1153 \ OS: Parameter error
#-1154 \ OS: Tape loading error

  \ ===========================================================
  \ Change log

  \ 2016-05-18: Modify format of messages.

  \ vim: filetype=soloforth
  \

