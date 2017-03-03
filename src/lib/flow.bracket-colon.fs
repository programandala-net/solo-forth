  \ flow.bracket-colon.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702280009

  \ -----------------------------------------------------------
  \ Description

  \ Incomplete implementation of quotations, as described in
  \ Forth-2012's RfD
  \ <http://www.forth200x.org/quotations-v3.txt>: No support
  \ for locals, not tested with `does>`.

  \ -----------------------------------------------------------
  \ Author

  \ Marcos Cruz (programandala.net), 2016.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ History

  \ 2016-11-25: First version.  Reference:
  \ <http://www.forth200x.org/quotations-v3.txt>.  Not tested
  \ with `does>`.  No support for locals.
  \
  \ 2017-01-05: Update `also assembler` to `assembler-wordlist
  \ >order`.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-02-27: Improve documentation.

( [: ;] )

: [:  \ Compilation: ( -- orig xt )
  postpone ahead  latestxt here lastxt !
  docolon [ assembler-wordlist >order ] call, [ previous ]
 ; immediate compile-only

  \ doc{
  \
  \ [:  \ Compilation: ( -- orig xt )
  \
  \ Suspend compiling to the current definition, start a new
  \ nested definition and compilation continues with this
  \ nested definition.
  \
  \ ``[:`` is an `immediate` and `compile-only` word.
  \
  \ See also: `;]`.
  \
  \ }doc

  \ XXX TODO -- Not supported:
  \
  \ Locals may be defined in the nested definition.  An
  \ ambiguous condition exists if a name is used that satisfies
  \ the following constraints:
  \
  \ 1. It is not the name of a currently visible local of the
  \ current quotation.
  \
  \ 2. It is the name of a local that was visible right before
  \ the start of the present quotation or any of the containing
  \ quotations.

: ;]  \ Compilation: ( orig xt1 -- )
      \ Execution:   ( -- xt2 )
  lastxt !  postpone exit  dup >resolve
  cell+ postpone literal ; immediate compile-only

  \ doc{
  \
  \ ;]
  \   Compilation: ( orig -- )
  \   Run-time: ( -- xt )
  \
  \ ``;]`` is an `immediate` and `compile-only` word.
  \
  \ Compilation: ( orig -- )
  \
  \ End the current nested definition, and resume compilation
  \ to the previous (containing) current definition. It appends
  \ the following run-time to the (containing) current
  \ definition:
  \
  \ Run-time: ( -- xt )
  \
  \ _xt_ is the execution token of the nested definition.
  \
  \ See also: `[:`.
  \
  \ }doc

  \ vim: filetype=soloforth