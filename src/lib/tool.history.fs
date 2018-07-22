  \ tool.history.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ XXX UNDER DEVELOPMENT

  \ Last modified: 201806041324
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ The command line history tool.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( history )

  \ XXX NEW -- upwards version, with back linked strings

  \ Every entry in the command line history has the following
  \ structure:
  \
  \ +0    : length byte
  \ +1..n : string
  \ +n+1  : address of +0

variable /history
  \ Size of the history space, where all strings are hold.

variable hp0
  \ Address of the bottom of the history.

variable hp
  \ The history pointer: Address of the free space in the
  \ history.

: used-history ( -- u ) hp0 @ hp @  - ;
  \ Used space _u_ in the history.

: unused-history ( -- n ) /history @ used-history - ;
  \ Unused space _n_ in the history.

: allot-history ( +n -- ) hp +! ;
  \ Reserve _+n_ bytes in the history.

: len>history ( len -- +n ) 1+ cell+ ;
  \ Convert a string length to space required to store it into
  \ the history.

: history>link ( ca -- a ) cell- ;
  \ Convert a history string address to its link field.

: history<history ( ca1 -- ca2 ) history>link far@ ;
  \ Convert a history string address to the previous one.

: history>history ( ca1 -- ca2 ) farcount + cell+ ;
  \ Convert a history string address to the next one.

: history>string ( ca1 -- ca2 len2 ) farcount >stringer ;
  \ Copy a history string to a string in the `stringer`.

-->

( history )

variable browsed-history
  \ Address of the history string being browsed.

: oldest-history? ( -- f ) browsed-history @ hp0 @ = ;
  \ Are we browsing the oldest string of history?

: browse-older-history ( -- )
  oldest-history? ?exit
  browsed-history @ history<history browsed-history ! ;
  \ Update the current history being browsed to the previous
  \ (older) one.

: newest-history? ( -- )
  browsed-history @ history>history hp @ =
  browsed-history @ hp @ =  or ;
  \ Are we browsing the newest string of history?

: browse-newer-history ( -- )
  newest-history? ?exit
  browsed-history @ history>history browsed-history ! ;
  \ Update the current history being browsed to the next
  \ (newer) one.

: init-history ( n -- )
  dup /history !  $FFFF swap -
  dup hp0 !  dup hp !  browsed-history !  0 hp0 @ farc! ;

1024 init-history  -->

( history )

: history-empty? ( -- f ) used-history 0= ;
  \ Is the history empty?

: allocate-history ( len -- ior )
  1+ dup /history @ > if  drop #-274 exit  then
    \ command line history overflow?
  \ ." allocate-history" \ XXX INFORMER
  0 ; \ XXX TMP
  \ Allocate space in the history for a string _len_ bytes
  \ long.
  \ XXX TODO -- remove older strings if needed

-->

( history )

: latest-history$ ( -- ca len )
  hp @ history<history history>string ;
  \ Return the latest string in the command line history,
  \ copied in the `stringer`.

: duplicated-history? ( ca len -- f ) latest-history$ str= ;
  \ Is string _ca len_ identical to the latest string in
  \ the command line history?

: longer-history? ( len -- f )
  len>history unused-history > ;
  \ Is _len_ too long?

: history, ( ca len -- )
  hp @ dup >r  over >r ( ca len ca1 ) ( R: len ca1 -- )
  farplace
  r> 1+ allot-history  r> hp @ far!  cell allot-history ;
  \ Add a string to the command line history.

: (>history ( ca len -- )
  dup 0= if  2drop exit  then
  2dup duplicated-history? if  2drop exit  then
  dup longer-history? if  dup allocate-history throw  then
  history,  hp @ browsed-history ! ;
  \ Save string _ca len_ into the command line history,
  \ provided the string is valid (not empty, not duplicated).
  \ Make room if necessary. Then update the pointer to the
  \ browsed history.

-->

( history )

: browsed-history$ ( -- ca len )
  browsed-history @ history>string ;
  \ Return the latest string in the command line history,
  \ copied in the `stringer`.

: get-history   ( -- ca ) browsed-history$ set-accept ;

: (history-up    ( -- ca )
  get-history browse-older-history ;
: (history-down ( -- ca )
  get-history browse-newer-history ;

-->

( history )

variable history
  \ A `variable` holding the current status of the command line
  \ history as a flag: on (true) or off (false).

: history-off ( -- )
  ['] 2drop ['] >history defer!
  ['] 0 ['] history-up   defer!
  ['] 0 ['] history-down defer!  history off ;
  \ Turn command line history off.

: history-on ( -- )
  ['] (>history ['] >history defer!
  ['] (history-up   ['] history-up   defer!
  ['] (history-down ['] history-down defer!  history on ;
  \ Turn command line history on.

-->

( history )

: .history ( -- )
  hp0 @ begin  dup hp @ u<  while
          dup history>string type cr  history>history
        repeat  drop ;

: .h ( -- ) hp0 dup hp @ - fardump ;

\ history-on

( history-xxx-old )

  \ XXX OLD -- downwards version

  \ 2016-03-07: Start.

  \ Command line history is implemented as a list of counted
  \ string at the top of a memory bank. It's the same bank
  \ where name fields are stored. Name fields are stored
  \ upwards from the bottom of the 16-KiB space; command line
  \ history grows downwards from the top.
  \
  \ The length of the every counted string is used as a link
  \ field to the previous string.  The bottom of the list is
  \ the highest address of the bank, and it holds one byte, the
  \ length of the first string stored in the history, or zero
  \ when the history is empty.
  \
  \ There's a maximum space usable for the history. When
  \ there's no free space left to store a new string, oldest
  \ strings are removed as necessary.

  \ 2016-03-08: XXX TODO -- Rewrite, simpler: grow upwards.

variable hp
  \ Pointer to the most recent string in the history.

$FFFF constant hp0
  \ Pointer to the bottom of the history, which contains a copy
  \ of the length of the first string.

variable /history  1024 /history !
  \ Size of the history space, where all strings are hold.

: init-hp0 ( -- ) 0 hp0 farc! ;

: history-bounds ( -- ca1 ca2 ) hp0 hp @ ;
  \ Return bottom of history _a1_ and address of the latest
  \ string _ca2:

: used-history ( -- u ) history-bounds - ;
  \ Used space _u_ in the history.

: unused-history ( -- n ) /history @ used-history - ;
  \ Unused space _n_ in the history.

-->

( history-xxx-old )

variable previously-browsed-history
  \ Address of the history string previously browsed.

variable currently-browsed-history
  \ Address of the history string being browsed.

: older-history ( -- )
  currently-browsed-history @ dup previously-browsed-history !
  farcount + currently-browsed-history ! ;
  \ Update the current history being browsed to the previous
  \ (older) one.

: newer-history ( -- )
  currently-browsed-history @
  previously-browsed-history @ currently-browsed-history !
  currently-browsed-history ! ;
  \ Update the current history being browsed to the next
  \ (newer) one.

: init-history ( -- )
  init-hp0  hp0 dup hp ! currently-browsed-history ! ;

init-history  -->

( history-xxx-old )

: allot-history ( +n -- ) negate hp +! ;
  \ Reserve _+n_ bytes in the history.

: history-empty? ( -- f ) history-bounds = ;
  \ Is the history empty?

: allocate-history ( len -- ior )
  1+ dup /history @ > if  drop #-274 exit  then
    \ command line history overflow?
  \ ." allocate-history" \ XXX INFORMER
  0 ; \ XXX TMP
  \ Allocate space in the history for a string _len_ bytes
  \ long.
  \ XXX TODO -- remove older strings if needed

: latest-history ( -- ca len ) hp @ farcount >stringer ;
  \ Return the latest string in the command line history,
  \ copied to the `stringer`.
  \ XXX OLD

: browsed-history ( -- ca len )
  currently-browsed-history @ farcount >stringer ;
  \ Return the latest string in the command line history,
  \ copied to the `stringer`.

: (history> ( -- ca len )
  browsed-history  dup 0= ?exit
                   dup 1+ negate allot-history
  history-empty? if  init-hp0  then ;  -->
  \ Get a string from the command line history, and return it
  \ as _ca len_ in the `stringer`.
  \ XXX TODO -- adapt the browser variables

( history-xxx-old )

: duplicated-history? ( ca len -- f ) latest-history str= ;
  \ Is string _ca len_ identical to the latest string in
  \ the command line history?

: too-long-for-history? ( len -- f ) 1+ unused-history > ;

: (>history ( ca len -- )
  dup 0= if  2drop exit  then
    \ If string is empty, do nothing.
  history-empty? if  dup hp0 farc!  then
    \ If history is empty, init its bottom with the length
    \ of the string.
  2dup duplicated-history? if  2drop exit  then
  dup too-long-for-history?
  if  dup allocate-history throw  then
    \ If there's no space left, allocate it.
  dup 1+ allot-history
  hp @ dup farplace currently-browsed-history ! ;
  \ Save string _ca len_ into the command line history.

-->

( history-xxx-old )

: get-history   ( -- ca ) browsed-history set-accept ;
: (history-up    ( -- ca ) get-history older-history ;
: (history-down ( -- ca ) get-history newer-history ;

variable history
  \ A `variable` holding the current status of the command line
  \ history as a flag: on (true) or off (false).

: history-off ( -- )
  \ XXX OLD
  \ ['] 2drop ['] >history defer!
  \ ['] s""   ['] history> defer!  history off ;
  \ XXX NEW
  ['] 0 ['] history-up   defer!
  ['] 0 ['] history-down defer!  history off ;
  \ Turn command line history off.

: history-on ( -- )
  \ XXX OLD
  \ ['] (>history ['] >history defer!
  \ ['] (history> ['] history> defer!  history on ;
  \ XXX NEW
  ['] (history-up   ['] history-up   defer!
  ['] (history-down ['] history-down defer!  history on ;
  \ Turn command line history on.

: .history ( -- )
  hp @  begin  dup hp0 <  while  farcount 2dup type cr +
        repeat  drop ;

\ history-on

  \ ===========================================================
  \ Change log

  \ 2016-05-05: Update `s=` to `str=`.
  \
  \ 2016-05-07: Make block titles compatible with `indexer`.
  \
  \ 2017-01-05: Update from old system bank to far memory.
  \
  \ 2017-03-12: Update the names of `stringer` words and
  \ mentions to it.
  \
  \ 2018-06-04: Update: remove trailing closing paren from
  \ word names. Link `variable` in documentation.

  \ vim: filetype=soloforth
