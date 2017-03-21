  \ exception.codes.0256.system.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201703212102
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ The Forth system error codes (range -4095..-256), except
  \ those reserved for the OS and the DOS.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( System error codes #-256..#-270 )

#-256 \ not a word nor a number
#-257 \ warning: is not unique
#-258 \ stack imbalance
#-259 \ trying to load from block 0
#-260 \ wrong digit
#-261 \ deferred word is uninitialized
#-262 \ assertion failed
#-263 \ execution only
#-264 \ definition not finished
#-265 \ loading only
#-266 \ off current editing block
#-267 \ warning: not present, though needed
#-268 \ needed, but not located
#-269 \ relative jump too long
#-270 \ text not found

( System error codes #-271..#-285 )

#-271 \ immediate word not allowed in this structure
#-272 \ array index out of range
#-273 \ invalid assembler condition
#-274 \ command line history overflow
#-275 \ wrong number
#-276 \ dictionary reached the zone of memory banks
#-277 \ needed, but not indexed
#-278 \ empty block found: quit indexing
#-279 \ user area overflow
#-280 \ user area underflow
#-281 \ escaped strings search-order overflow
#-282 \ escaped strings search-order underflow
#-283 \ assembly label number out of range
#-284 \ assembly label number already used
#-285 \ too many unresolved assembly label references

( System error codes #-286..#-300 )

#-286 \ not located
#-287 \ wrong number of drives
#-288 \ too many files open
#-289 \ input source exhausted
#-290 \ invalid UDG scan
#-291 \
#-292 \
#-293 \
#-294 \
#-295 \
#-296 \
#-297 \
#-298 \
#-299 \
#-300 \

  \ ===========================================================
  \ Change log

  \ 2016-05-18: Modify format of messages.
  \
  \ 2016-11-25: Rename "required" to "needed", because the word
  \ used is `need` and its family.
  \
  \ 2016-12-23: Add codes #-281 and #-282 for escaped strings
  \ search order.
  \
  \ 2016-12-26: Add codes #-283 and #-285 for assembler labels.
  \
  \ 2017-01-11: Add code #-286.
  \
  \ 2017-02-09: Add code #-287, needed by `set-block-drives`.
  \
  \ 2017-03-08: Add code #-288, needed by `create-file` and
  \ `open-file`.
  \
  \ 2017-03-19: Add code #-289, needed by `parse-name-thru`.
  \ Add code #-290, needed by `udg-scan>number`.
  \
  \ 2017-03-21: Improve text of #-285.

  \ vim: filetype=soloforth
