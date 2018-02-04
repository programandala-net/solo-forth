  \ flow.bracket-colon.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201802041946
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Incomplete implementation of quotations, as described in
  \ Forth-2012's RfD
  \ <http://www.forth200x.org/quotations-v3.txt>: No support
  \ for locals, not tested with `does>`.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( [: ;] )

: [: \ Compilation: ( -- orig xt )
  postpone ahead  latestxt here lastxt !
  docolon [ assembler-wordlist >order ] call, [ previous ]
  ; immediate compile-only

  \ doc{
  \
  \ [: "bracket-colon"
  \   Compilation: ( -- orig xt )
  \

  \ Start a quotation.
  \
  \ Suspend compiling to the current definition, start a new
  \ nested definition and compilation continues with this
  \ nested definition. Return _orig_ and the execution token
  \ _xt_ of of the host definition, both to be consumed by
  \ `;]`.
  \
  \ NOTE: Locals are not supported yet.
  \
  \ ``[:`` is an `immediate` and `compile-only` word.
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

: ;]
  \ Compilation: ( orig xt1 -- )
  \ Run-time:    ( -- xt2 )
  lastxt !  postpone exit  dup >resolve
  cell+ postpone literal ; immediate compile-only

  \ doc{
  \
  \ ;] "semicolon-bracket"
  \   Compilation: ( orig xt1 -- )
  \   Run-time:    ( -- xt2 )
  \

  \ End a quotation started by `[:`.
  \
  \ ``;]`` is an `immediate` and `compile-only` word.
  \
  \ Compilation:
  \
  \ End the current nested definition, and resume compilation
  \ to the previous (containing) current definition, identified
  \ by _xt1_. Resolve the branch from _orig_ left by `[:`.
  \ Append the following run-time to the (containing) current
  \ definition:
  \
  \ Run-time:
  \
  \ _xt2_ is the execution token of the nested definition.
  \
  \ }doc

  \ ===========================================================
  \ Change log

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
  \
  \ 2017-03-18: Improve documentation.
  \
  \ 2017-05-07: Improve documentation.
  \
  \ 2018-02-04: Improve documentation: add pronunciation to
  \ words that need it.

  \ vim: filetype=soloforth
