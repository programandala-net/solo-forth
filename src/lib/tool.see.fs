  \ tool.see.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201806041101
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ The `see` utility.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

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

need name>body need case need >oldest-name
need recurse need >body need body> need [undefined] need d=
need cond need thens need defer@

: >oname. ( xt -- ) >oldest-name ?dup if   .name
                                      else ." :noname" then ;

: body>oname. ( dfa -- nt ) body> >oname. ;

variable see-level  see-level off
  \ Depth of nesting.

variable see-address
  \ Address in the body of the colon word which is being
  \ decoded.

: (indent ( a -- ) cr u. see-level @ 2* spaces ;

: indent ( -- ) see-address @ (indent ;

: indent+ ( -- ) 1 see-level +! indent ;

: see-branch ( a1 -- a2 ) cell+ dup @ u. ;

: see-literal ( a1 -- a2 ) cell+ dup @ . ;

: see-2literal ( a1 -- a2 ) cell+ dup 2@ d. cell+ ;

: see-cliteral ( a1 -- a2 ) cell+ dup c@ . 1- ; -->

( see )

: see-sliteral ( a1 -- a2 )
  cell+ dup count '"' emit type '"' emit  dup c@ + 1- ;

: see-compile   ( a1 -- a2 ) cell+ dup @ >oname. ;

: see-special ( a1 -- a1 | a2 ) dup @ case
    ['] compile of see-compile  endof
    ['] lit     of see-literal  endof
    ['] 2lit    of see-2literal endof
    ['] clit    of see-cliteral endof
    ['] slit    of see-sliteral endof
    ['] branch  of see-branch   endof
    ['] 0branch of see-branch   endof
    ['] ?branch of see-branch   endof
    ['] (do     of see-branch   endof
    ['] (?do    of see-branch   endof
    ['] (."     of see-sliteral endof -->

( see )

    [undefined] cslit   ?\ ['] cslit   of see-sliteral endof
    [undefined] (abort" ?\ ['] (abort" of see-sliteral endof
    [undefined] -branch ?\ ['] -branch of see-branch   endof
  endcase ;

: colon-end? ( xt -- f ) dup ['] exit = swap ['] (;code = or ;
  \ Is _xt_ the end of colon definition?

: see-usage ( -- ) cr ." SPACE=more Q=quit other=deeper" cr ;

  \ doc{
  \
  \ see-usage ( -- )
  \
  \ Display the usage of `see`. This word is executed when
  \ `manual-see` contains non-zero.
  \
  \ }doc

variable manual-see manual-see on
  \ XXX TODO -- Rename.
  \ XXX UNDER DEVELOPMENT

  \ doc{
  \
  \ manual-see ( -- a )
  \
  \ A variable. _a_ is the address of a cell containing a flag.
  \ When the flag is non-zero, the decompilation of colon words
  \ done by `see` can be controlled manually with some keys,
  \ which are displayed at the start of the process.
  \
  \ See: `see-usage`.
  \
  \ }doc

: ?see-usage ( -- ) manual-see @ if see-usage then ; -->

( see )

: (see-colon-body ( dfa -- )
  begin  ( dfa+n ) dup see-address !
         dup @ ( dfa+n xt ) dup colon-end? 0=
  while  \ ( dfa+n xt )
         >body ( dfa+n dfa' ) dup indent+ body>oname.
         manual-see @
         if   key case 'q' of empty-stack quit endof
                       bl  of drop             endof
                       swap recurse
              endcase
         else drop
         then see-special cell+ -1 see-level +!
  repeat indent >oname. drop ; -->
  \ XXX TODO -- Make recursion work also with non-colon words.

( see )

: see-colon-body ( dfa -- )
  ?see-usage dup body> see-address !
             dup indent ." : " body>oname. (see-colon-body ;

  \ doc{
  \
  \ see-colon-body ( dfa -- )
  \
  \ Decode the colon word's definition whose body is _dfa_.
  \ ``see-colon-body`` is a factor of `see-colon`.
  \
  \ See: `see`, `see-colon-body>`, `see-xt`, `see-usage`.
  \
  \ }doc

: ucreate-cf? ( c a -- )
  $CD [ ' (user >body 2 cells + ] literal d= ;
  \ Is _c a_ the code field of a word created by `ucreate`,
  \ `user`, `2user` or `(user`?
  \
  \ WARNING: the code address is calculated from the code field
  \ address of `(user`, after its current code.

: colon-cf? ( c a -- ) $CD docolon d= ;
  \ Is _c a_ the code field of a word created by `:`?

: constant-cf? ( c a -- ) $CD ['] @ d= ;
  \ Is _c a_ the code field of a word created by `constant`?

: 2constant-cf? ( c a -- ) $CD ['] 2@ d= ;
  \ Is _c a_ the code field of a word created by `2constant`?

: cconstant-cf? ( c a -- ) $CD ['] c@ d= ;
  \ Is _c a_ the code field of a word created by `cconstant`?

: create-cf? ( c a -- ) $CD ['] noop d= ;
  \ Is _c a_ the code field of a word created by `create`,
  \ `variable` or `2variable`?

: defer-cf? ( c a -- ) drop $C3 = ;
  \ Is _c a_ the code field of a word created by `defer`?

: see-code ( nt -- ) dup name> (indent ." code " .name ;

: see-constant ( nt -- )
  dup name>body dup (indent @ . ." constant " .name ; -->

( see )

: see-cconstant ( nt -- )
  dup name>body dup (indent c@ . ." cconstant " .name ;

: see-2constant ( nt -- )
  dup name>body dup (indent 2@ d. ." 2constant " .name ;

: see-create ( nt -- )
  dup name>body (indent ." create " .name ;

: see-colon ( nt -- ) name>body see-colon-body ;

: see-defer ( nt -- ) dup name> dup (indent
                          ." defer " swap .name
                          ." \ action: " defer@ >oname. ;

: see-ucreate ( nt -- )
  dup name> execute (indent ." ucreate " dup .name
  ." \ index: " name>body c@ . ; -->

( see )

: cfa@ ( cfa -- c a ) dup c@ swap 1+ @ ;
  \ Fetch the contents of code field address _cfa_, returning
  \ Z80 opcode _c_ ($CD for ``call`` or $C3 for ``jp``) and
  \ address _a_.

: (see ( nt xt -- )
  cfa@ cond 2dup  constant-cf? if 2drop see-constant  else
            2dup 2constant-cf? if 2drop see-2constant else
            2dup cconstant-cf? if 2drop see-cconstant else
            2dup     defer-cf? if 2drop see-defer     else
            2dup    create-cf? if 2drop see-create    else
            2dup   ucreate-cf? if 2drop see-ucreate   else
                     colon-cf? if       see-colon     else
            see-code thens ;
  \ Decode the word identified by _nt_ and _xt_.
  \ A common factor of `see-xt` and `see-name`.

: see-xt ( xt -- ) dup >oldest-name swap (see ;

  \ doc{
  \
  \ see-xt ( xt -- ) "see-x-t"
  \
  \ Decode the word's definition _xt_.
  \
  \ The listing can be paused with the space bar, then stopped
  \ with the return key or resumed with any other key.
  \
  \ See: `see`, `see-name`, `see-colon-body`, `see-colon-body>`.
  \
  \ }doc

: see-name ( nt -- ) dup name> (see ;

  \ doc{
  \
  \ see-name ( nt -- )
  \
  \ Decode the word's definition _nt_.
  \
  \ ``see-name`` is a factor of `see`.
  \
  \ See: `see-xt`, `see-colon-body`, `see-colon-body>`.
  \
  \ }doc

: see ( "name" -- )
  defined ( nt | 0 ) dup 0= -13 ?throw see-level off see-name ;

  \ doc{
  \
  \ see ( "name" -- )
  \
  \ Decode the word's definition _name_.  At the moment ``see``
  \ works only with colon definitions.
  \
  \ Origin: Forth-94 (TOOLS), Forth-2012 (TOOLS).
  \
  \ See: `see-name`, `see-xt`, `see-colon-body`, `see-colon-body>`.
  \
  \ }doc

( see-colon-body> )

need see

: see-colon-body> ( a -- )
  dup body> see-address ! (see-colon-body ;

  \ doc{
  \
  \ see-colon-body> ( a -- ) "see-colon-body-from"
  \
  \ Decode the colon word's definition from _a_, which is part
  \ of its body. ``see-colon-body>`` is useful to decode words
  \ that use `exit` in the midle of the definition, because
  \ `see` stops at the first `exit` found.
  \
  \ See: `see-colon-body`, `see-xt`, `see-name`.
  \
  \ }doc

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
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-03-17: Support `(abort")`, provided it is defined when
  \ `see` is compiled.
  \
  \ 2017-09-09: Update notation "pfa" to the standard "dfa".
  \
  \ 2017-12-09: Need `[undefined]`, which is moved to the
  \ library.
  \
  \ 2017-12-12: Need `>name`, which has been moved to the
  \ library.
  \
  \ 2017-12-18: Replace `>name` with `>oldest-name`.
  \
  \ 2018-01-01: Rename `see-body` `see-colon-body`.  Rename
  \ `see-body-from` `see-colon-body>`. Refactor. Remove
  \ `.see-body-name`. Recognize constants, double constants,
  \ character constants, words created by `create` and
  \ `ucreate`, deferred words and code words.
  \
  \ 2018-01-03: Replace `body>name` with `body>oname`.
  \
  \ 2018-01-04: Replace `body>oname .name` with `body>oname.`,
  \ using `>oname.` to handle unnamed colon words.
  \
  \ 2018-01-06: Start implementing `manual-see`.
  \
  \ 2018-01-07: Finish implementation of `manual-see`.
  \
  \ 2018-03-09: Add words' pronunciaton.
  \
  \ 2018-06-04: Update: remove trailing closing paren from
  \ word names.

  \ vim: filetype=soloforth
