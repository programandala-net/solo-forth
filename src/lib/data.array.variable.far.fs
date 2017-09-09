  \ data.array.variable.far.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201709091154
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words to create and manage 1-dimension single-cell,
  \ double-cell and character variables arrays, which behave
  \ like `variable`, in far memory.

  \ Usage example of a single-cell variables array:

    \ 4 faravariable bar
    \ 10 0 bar far!  20 1 bar far!  30 2 bar far!  40 3 bar far!
    \ 3 bar far@ .
    \ 0 bar far@ .
    \ 123 3 bar !
    \ 3 bar far@ .
    \ 1 3 bar +!
    \ 3 bar far@ .

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( faravariable far2avariable farcavariable )

[unneeded] faravariable ?( need farallot need array>

: faravariable ( n "name" -- )
  create  np@ , cells farallot
  does> ( n -- a ) ( n dfa ) @ array> ; ?)

  \ doc{
  \
  \ faravariable ( n "name" -- )
  \
  \ Create, in far memory, a 1-dimension single-cell variables
  \ array _name_ with _n_ elements and the execution semantics
  \ defined below.
  \
  \ _name_ execution:
  \
  \ name ( n -- a )
  \
  \ Return far-memory address _a_ of element _n_.
  \
  \ See also: `far2avariable`, `farcavariable`, `avariable`.
  \
  \ }doc

[unneeded] far2avariable ?( need farallot need 2array>

: far2avariable ( n "name" -- )
  create  np@ , [ 2 cells ] cliteral * farallot
  does> ( n -- a ) ( n dfa ) @ 2array> ; ?)

  \ doc{
  \
  \ far2avariable ( n "name" -- )
  \
  \ Create, in far memory, a 1-dimension double-cell variables
  \ array _name_ with _n_ elements and the execution semantics
  \ defined below.
  \
  \ _name_ execution:
  \
  \ name ( n -- a )
  \
  \ Return far-memory address _a_ of element _n_.
  \
  \ See also: `faravariable`, `farcvariable`, `2avariable`.
  \
  \ }doc

[unneeded] cavariable ?( need farallot

: farcavariable ( n "name" -- )
  create  np@ , farallot
  does> ( n -- ca ) ( n dfa ) @ + ; ?)

  \ doc{
  \
  \ farcavariable ( n "name" -- )
  \
  \ Create, in far memory, a 1-dimension character variables
  \ array _name_ with _n_ elements and the execution semantics
  \ defined below.
  \
  \ _name_ execution:
  \
  \ name ( n -- ca )
  \
  \ Return far-memory address _ca_ of element _n_.
  \
  \ See also: `faravariable`, `far2variable`, `cavariable`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2017-01-18: Start, based on the module
  \ <data.array.variable.fsb>.
  \
  \ 2017-02-16: Fix typo in documentation.
  \
  \ 2017-02-17: Fix markup in documentation.  Update cross
  \ references.
  \
  \ 2017-02-26: Update "hp" notation to "np", after the changes
  \ in the kernel.
  \
  \ 2017-09-09: Update notation "pfa" to the standard "dfa".

  \ vim: filetype=soloforth
