  \ exception.codes.0001.standard.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702220020
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ The standard Forth error codes (range -255..-1).

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( Standard error codes #-01..#-15 )

#-01 \ ABORT
#-02 \ ABORT"
#-03 \ stack overflow
#-04 \ stack underflow
#-05 \ return stack overflow
#-06 \ return stack underflow
#-07 \ do-loops nested too deeply during execution
#-08 \ dictionary overflow
#-09 \ invalid memory address
#-10 \ division by zero
#-11 \ result out of range
#-12 \ argument type mismatch
#-13 \ undefined word
#-14 \ interpreting a compile-only word
#-15 \ invalid FORGET

( Standard error codes #-16..#-30 )

#-16 \ attempt to use zero-length string as a name
#-17 \ pictured numeric output string overflow
#-18 \ parsed string overflow
#-19 \ definition name too long
#-20 \ write to a read-only location
#-21 \ unsupported operation
#-22 \ control structure mismatch
#-23 \ address alignment exception
#-24 \ invalid numeric argument
#-25 \ return stack imbalance
#-26 \ loop parameters unavailable
#-27 \ invalid recursion
#-28 \ user interrupt
#-29 \ compiler nesting
#-30 \ obsolescent feature

( Standard error codes #-31..#-45 )

#-31 \ >BODY used on non-CREATEd definition
#-32 \ invalid name argument
#-33 \ block read exception
#-34 \ block write exception
#-35 \ invalid block number
#-36 \ invalid file position
#-37 \ file I/O exception
#-38 \ non-existent file
#-39 \ unexpected end of file
#-40 \ invalid BASE for floating point conversion
#-41 \ loss of precision
#-42 \ floating-point divide by zero
#-43 \ floating-point result out of range
#-44 \ floating-point stack overflow
#-45 \ floating-point stack underflow

( Standard error codes #-46..#-60 )

#-46 \ floating-point invalid argument
#-47 \ compilation word list deleted
#-48 \ invalid POSTPONE
#-49 \ search-order overflow
#-50 \ search-order underflow
#-51 \ compilation word list changed
#-52 \ control-flow stack overflow
#-53 \ exception stack overflow
#-54 \ floating-point underflow
#-55 \ floating-point unidentified fault
#-56 \ QUIT
#-57 \ exception in sending or receiving a character
#-58 \ [IF], [ELSE], or [THEN] exception
#-59 \ ALLOCATE
#-60 \ FREE

( Standard error codes #-61..#-75 )

#-61 \ RESIZE
#-62 \ CLOSE-FILE
#-63 \ CREATE-FILE
#-64 \ DELETE-FILE
#-65 \ FILE-POSITION
#-66 \ FILE-SIZE
#-67 \ FILE-STATUS
#-68 \ FLUSH-FILE
#-69 \ OPEN-FILE
#-70 \ READ-FILE
#-71 \ READ-LINE
#-72 \ RENAME-FILE
#-73 \ REPOSITION-FILE
#-74 \ RESIZE-FILE
#-75 \ WRITE-FILE

( Standard error codes #-76..#-79 )

#-76 \ WRITE-LINE
#-77 \ malformed xchar
#-78 \ SUBSTITUTE
#-79 \ REPLACES
#-80 \
#-81 \
#-82 \
#-83 \
#-84 \
#-85 \
#-86 \
#-87 \
#-88 \
#-89 \
#-90 \

  \ ===========================================================
  \ Change log

  \ 2016-05-18: Modify format of messages.

  \ vim: filetype=soloforth
