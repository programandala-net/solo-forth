  \ os.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201803232320
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words related to the OS.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( os-used os-unused first-stream last-stream stream> stream? )

unneeding os-used unneeding os-unused and ?(

code os-used ( -- u )
  C5 c, 1F1A call, E1 c, C5 c, 44 c, 4D c, jpnext, end-code
  \ push bc ; save Forth IP
  \ call $1F1A ; BC = OS unused memory
  \ pop hl ; preserve Forth IP
  \ push bc ; push result
  \ ld b,h
  \ ld c,l ; restore Forth IP
  \ _jp_next

  \ doc{
  \
  \ os-used ( -- u ) "o-s-used"
  \
  \ _u_ is the amount of the 64-KiB space, including ROM, video
  \ memory, system variables and reserved zones, that can not
  \ used by the OS and the BASIC interpreter.
  \
  \ See: `os-unused`.
  \
  \ }doc

: os-unused ( -- u ) $FFFF os-used - 1+ ; ?)

  \ doc{
  \
  \ os-unused ( -- u ) "o-s-unused"
  \
  \ _u_ is the amount of unused space by the OS and the BASIC
  \ interpreter.
  \
  \ See: `unused`, `os-used`.
  \
  \ }doc

unneeding first-stream ?\ -3 constant first-stream

  \ doc{
  \
  \ first-stream ( -- n )
  \
  \ _n_ is the number of the first stream.
  \
  \ See: `last-stream`, `os-strms`, `stream>`, `stream?`.
  \
  \ }doc

unneeding last-stream ?\ 15 cconstant last-stream

  \ doc{
  \
  \ last-stream ( -- n )
  \
  \ _n_ is the number of the last stream.
  \
  \ See: `first-stream`, `os-strms`, `stream>`, `stream?`.
  \
  \ }doc

unneeding stream> ?( need first-stream need os-strms

: stream> ( n -- a )
  [ first-stream negate ] xliteral + cells os-strms + ; ?)

  \ doc{
  \
  \ stream> ( n -- a )
  \
  \ Convert stream number _n_ to address _a_ of its
  \ corresponding element in `os-strms`.
  \
  \ See: `first-stream`, `last-stream`, `stream?`.
  \
  \ }doc

unneeding stream? ?( need first-stream need last-stream
                     need stream>

: stream? ( -- false | n true )
  last-stream first-stream ?do
    i stream> @ 0= if i true unloop exit then
  loop false ; ?)

  \ doc{
  \
  \ stream? ( -- false | n true )
  \
  \ If there's a closed stream, return its number _n_ and
  \ _true_; otherwise return _false_.
  \
  \ See: `os-strms`, `.os-strms`, `first-stream`,
  \ `last-stream`, `stream>`.
  \
  \ }doc

( chan> chan>id .os-strms )

unneeding chan> ?( need os-chans

: chan> ( n -- a ) 1- os-chans @ + ; ?)

  \ doc{
  \
  \ chan> ( n -- a ) "chan-to"
  \
  \ Convert channel offset _n_ in `os-chans`, fetched from an
  \ element of `os-strms`, to its address _a_.
  \
  \ See: `chan>id`, `os-chans`.
  \
  \ }doc

unneeding chan>id ?( need chan>

: chan>id ( n -- c ) chan> 4 + c@ ; ?)

  \ doc{
  \
  \ chan>id ( n -- c ) "chan-to-id"
  \
  \ Convert channel offset _n_ in `os-chans`, fetched from an
  \ element of `os-strms`, to its character identifier _c_.
  \
  \ See: `chan>`, `os-chans`.
  \
  \ }doc

unneeding .os-strms ?( need os-chans need os-strms need stream>
                       need first-stream need last-stream
                       need chan> need chan>id

: .os-strms ( -- )
  last-stream first-stream ?do
    '#' emit i .  i stream> @ ?dup
    if   dup u.
         dup chan>id ." -- channel '" emit ." ' at " chan> u.
    else ." Not attached"
    then cr
  loop ; ?)

  \ doc{
  \
  \ .os-strms ( -- ) "dot-o-s-streams"
  \
  \ Display the contents of `os-strms`.
  \
  \ See: `.os-chans`, `first-stream`, `last-stream`, `stream?`.
  \
  \ }doc

( .os-chans )

need os-chans need >graphic-ascii-char need nuf?

: .os-chans ( -- )
  os-chans @
  begin  dup c@ 128 <>
  while                dup u.
         ." Out:"      dup @ u.
         ." In:" cell+ dup @ u.
         ." Id:" cell+ dup c@ >graphic-ascii-char emit 1+ cr
         nuf? if drop exit then
  repeat drop ;
  \
  \ XXX TODO -- The 128 check, used by G+DOS,  does not work
  \ after a disk channel has been added. Find out how to detect
  \ the end of the table.

  \ doc{
  \
  \ .os-chans ( -- ) "dot-o-s-chans"
  \
  \ Display the contents of `os-chans`.
  \
  \ See: `.os-strms`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2018-03-23: Start. Add `.os-strms`, `.os-chans`, `chan>`,
  \ `chan>id`, `stream>`, `stream?`, `os-used`, `os-unused`.

  \ vim: filetype=soloforth
