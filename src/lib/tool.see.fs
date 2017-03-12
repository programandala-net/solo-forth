  \ tool.see.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702221550
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ The `see` utility.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ Credit

  \ This code was adapted and improved from Afera's `decode`
  \ (2015), by the same author.  The Afera version was adapted
  \ and deeply modified from: Z80 CP/M fig-Forth 1.1g
  \ (adaptative version by EHR), modified by Dennis L. Wilson.
  \ The original code was written by Robert Dudley Ackerman,
  \ published on Forth Dimensions (volume 4, number 2, page 28,
  \ 1982-07).

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( see )

get-current  also forth definitions decimal

need body>name need name>body need case
need recurse need >body need body>

variable see-level  see-level off \ depth of nesting
variable see-address  \ in the word being decoded

: indent ( -- ) cr see-address @ u. see-level @ 2* spaces ;

: indent+ ( -- ) 1 see-level +! indent ;

: see-branch    ( a1 -- a2 ) cell+ dup @ u. ;

: see-literal   ( a1 -- a2 ) cell+ dup @ . ;

: see-2literal   ( a1 -- a2 ) cell+ dup 2@ d. cell+ ;

: see-cliteral ( a1 -- a2 ) cell+ dup c@ . 1- ;

: see-sliteral ( a1 -- a2 )
  cell+ dup count '"' emit type '"' emit  dup c@ + 1- ;

: see-compile   ( a1 -- a2 ) cell+ dup @ >name .name ;  -->

( see )

: see-special ( a1 -- a1 | a2 ) dup @ case
    ['] compile   of  see-compile    endof
    ['] lit       of  see-literal    endof
    ['] 2lit      of  see-2literal   endof
    ['] clit      of  see-cliteral   endof
    ['] slit      of  see-sliteral   endof
    ['] branch    of  see-branch     endof
    ['] 0branch   of  see-branch     endof
    ['] ?branch   of  see-branch     endof
    ['] (do)      of  see-branch     endof
    ['] (?do)     of  see-branch     endof
    ['] (.")      of  see-sliteral   endof
    [undefined] cslit ?\ ['] cslit   of see-sliteral endof
    [undefined] -branch ?\ ['] -branch of see-branch   endof
  endcase ;  -->

( see )

: colon-end? ( xt -- f )
  dup  ['] exit =  swap ['] (;code) =  or ;
  \ Is _xt_ the end of colon definition?

: colon-xt? ( xt -- f )
  dup c@ $CD = swap 1+ @ docolon = and ;
  \ Is _xt_ a colon definition?
  \ First, its first byte must be $CD (the Z80 call opcode);
  \ second, its jump address must be the colon interpreter.

defer colon-body? ( pfa -- f )

: (colon-body?) ( pfa -- f ) body> colon-xt? ;
  \ Is _a_ the body of a colon definition?

defer .see-body-name ( pfa -- )

: (.see-body-name) ( pfa -- )
  indent  ." : " body>name .name ;

: be-see-body ( -- )
  ['] (colon-body?) ['] colon-body? defer!
  ['] (.see-body-name) ['] .see-body-name defer! ; be-see-body

: no-colon-check ( pfa -- true ) drop true ;  -->

  \ : variable-xt? ( xt -- f )
  \   dup c@ $CD = swap 1+ @ docreate = and ;
  \   \ Does _xt_ belongs to a variable definition?
  \   \ First, its first byte must be $CD (the Z80 call opcode);
  \   \ second, its jump address must be the variable interpreter.
  \ XXX TODO --

  \ : variable-body? ( a -- f ) body> variable-xt? ;  -->
  \   \ Is _a_ the body of a variable definition?
  \ XXX TODO --

( see )

: see-body ( pfa -- )
  dup colon-body?  if
    dup body> see-address ! dup .see-body-name  be-see-body
    begin   ( pfa+n ) dup see-address !
            dup @ ( pfa+n xt ) dup colon-end? 0=
    while  \ high level & not end of colon definition
      \ ( pfa+n xt )
      >body ( pfa+n pfa' ) dup indent+  body>name .name
      key case  'q' of  sp0 @ sp! quit  endof
                bl  of  drop            endof
                swap recurse  \ default
          endcase  see-special  cell+  -1 see-level +!
    repeat  indent >name .name
            \ show the last word
  else  ." Not a colon definition."  then drop ;  -->

  \ doc{
  \
  \ see-body ( pfa -- )
  \
  \ Decode the colon word's definition whose body is _pfa_.
  \ This word is a factor of `see`.
  \
  \ See also: `see-body-from`, `see-xt`.
  \
  \ }doc

( see )

: see-usage ( -- )
     \  <------------------------------>
  cr ." Keys: space=more, q=quit, other=deeper." cr ;

: see ( "name" -- )
  defined ( nt | 0 ) dup 0= -13 ?throw  see-usage
  name>body  see-level off  see-body ;

  \ doc{
  \
  \ see ( "name" -- )
  \
  \ Decode the word's definition _name_.
  \ At the moment this word works only with colon definitions.
  \
  \ Origin: Forth-94 (TOOLS), Forth-2012 (TOOLS).
  \
  \ See also: `see-xt`, `see-body`, `see-body-from`.
  \
  \ }doc

previous set-current

( see-body-from see-xt )

get-current  also forth definitions

[unneeded] see-body-from ?( need see

: see-body-from ( a -- )
  ['] drop ['] .see-body-name defer!
  ['] no-colon-check ['] colon-body? defer!  see-body ; ?)

  \ doc{
  \
  \ see-body-from ( a -- )
  \
  \ Decode the colon word's definition from _a_, which is part
  \ of its body. This word is useful to decode words that use
  \ `exit` in the midle of the definition, because `see` stops
  \ at the first `exit` found.
  \
  \ See also: `see-body`, `see-xt`.
  \
  \ }doc

[unneeded] see-xt ?( need see need nuf?

: see-xt ( xt -- ) dup colon-xt?  if  see-level off
    dup see-address ! indent  ." : " dup >name .name >body
    begin   ( pfa+n ) dup see-address !
            dup @ ( pfa+n xt ) dup colon-end? 0=  nuf? 0= and
    while   >body ( pfa+n pfa' ) indent+ body>name .name
            see-special  cell+  -1 see-level +!
    repeat  indent >name .name
  else  ." Not a colon definition."  then drop ; ?)

  \ XXX TODO -- factor with `see-body`

  \ doc{
  \
  \ see-xt ( xt -- )
  \
  \ Decode the word's definition _xt_.
  \ At the moment this word works only with colon definitions.
  \
  \ The listing can be paused with the space bar, then stopped
  \ with the return key or resumed with any other key.
  \
  \ See also: `see`, `see-body`, `see-body-from`.
  \
  \ }doc

previous set-current

  \ ===========================================================
  \ Change log

  \ 2015-06-05: Copied from Afera. First changes to adapt it.
  \
  \ 2015-06-19: Added `?branch`.
  \
  \ 2015-07-23: Fix: `clit` was not included in the recognized
  \ special cases.
  \
  \ 2015-08-14: Fixed a recent bug: `sp0 sp!` was used when
  \ quitting, instead of `sp0 @ sp!`!
  \
  \ 2015-10-09: Fix: `slit` was missing from the special cases.
  \
  \ 2015-12-21: Fixed `decode-special` after the Forth-83
  \ version of `do loop`: now the branch address is after `do`
  \ or `?do`; also added `-branch` to it, in case it is already
  \ defined during the compilation of `decode-special`.
  \
  \ 2015-12-24: Start converting from ITC to DTC.
  \
  \ 2016-04-15: Fixed `decode-compile`, which had not been
  \ adapted from ITC to DTC.
  \
  \ 2016-04-17: Added the requisite of `recurse`, which is not
  \ in the kernel anymore. Updated the history from the
  \ development history of the project.
  \
  \ 2016-04-24: Add support for `2lit`.
  \
  \ 2016-04-24: Remove `[char]`, which has been moved to the
  \ library.
  \
  \ 2016-05-17: Need `>body` and `body>`, which has been moved
  \ to the library.
  \
  \ 2016-05-18: Improve `colon-cfa?`. Compact `decode-special`.
  \ Fix `decode`, which showed the usage instructions before
  \ checking the word.
  \
  \ 2016-11-17: Remove unused `need [if]`.
  \
  \ 2016-11-23: Rename `decode` to `see`; rename all words and
  \ the module accordingly. Compact the code, saving one block.
  \ Document the interface words.
  \
  \ 2016-11-24: Add `see-xt` and `see-body-from`. Make them
  \ individually accessible for `need`.
  \
  \ 2016-12-03: Preserve the compilation wordlist and the
  \ search order. Useful when the tool is needed during the
  \ development of a program.
  \
  \ 2017-01-06: Fix: the compilation word list was left on the
  \ stack. Improve: also `see-body-from` and `see-xt` are
  \ compiled in the Forth word list.
  \
  \ 2017-01-18: Type compiled strings between quotes. This is
  \ useful when they have leading or trailing spaces.
  \
  \ 2017-02-17: Update cross references.

  \ vim: filetype=soloforth
