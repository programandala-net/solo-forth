  \ flow.doer.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201709091154
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Leo Brodie's `doer make` construct.

  \ ===========================================================
  \ Credit

  \ Original code by Leo Brodie, 1983, published on _Thinking
  \ Forth_, Appendix A. Public domain.

  \ This version was adapted from PFE by Marcos Cruz
  \ (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( doer )

need >body

: doer-noop ( -- ) ;

: doer ( "name" -- )
  create  ['] doer-noop >body ,  does> ( dfa ) @ >r ;
  \ Define a word whose action is vectorable.

: (make)
  \ Stuff the address of further code into the data field
  \ of a doer word.
  r> dup cell+ dup cell+
    ( a1 a2 a2 )
    \ a1 = address of an optional continuation after `;and`,
    \      or zero
    \ a2 = address of the doer word
    \ a3 = address of the code to associate the doer word with
  swap @ >body !
    \ Get the dfa of the doer word and store the code address
    \ into it.
  @ ?dup if  >r  then ;
    \ Manage the optional continuation after `;and`.

variable >;and
  \ Hold the address of optional continuation pointer.

: make
  \ Used interpretively:
  \   make doer-name forth-code ;
  \ Or inside a definition:
  \   : definition  make doer-name forth-code ;
  compiling? if     postpone (make)  here >;and ! 0 ,
             else   here ' >body ! ]  then ; immediate

: ;and ( -- ) postpone exit  here >;and @ ! ; immediate
  \ Allow continuation of the "making" definition.

: undo ( "name" -- ) ['] doer-noop >body  ' >body ! ;
  \ Make the doer word "name" safe to execute.

  \ ===========================================================
  \ Change log

  \ 2016-05-17: Need `>body`, which has been moved to the
  \ library.
  \
  \ 2016-11-27: Move `doer-test` to the tests module.
  \
  \ 2017-02-17: Update notation "behaviour" to "action".
  \
  \ 2017-09-09: Update notation "pfa" to the standard "dfa".

  \ vim: filetype=soloforth
