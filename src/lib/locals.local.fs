  \ locals.local.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201705220133
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ A simple solution to use an ordinary variable as local,
  \ saving its current value on the return stack and restoring
  \ it at the end.

  \ ===========================================================
  \ Authors

  \ Original code by Henning Hanseng, published on Forth
  \ Dimensions (volume 9, number 5, page 6, 1988-01).
  \
  \ Adapted by Marcos Cruz (programandala.net), 2015, 2016,
  \ 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( local )

here ] ( R: a x -- ) 2r> swap ! exit [
  \ Exit point of `local` to restore the value _x_ of variable
  \ _a_.

: local \ Definition: ( a -- a )
        \ Run-time:   ( a0 -- ) ( R: a1 -- a0 x a a1 )
          \ a   = address of colon code to be executed
          \       to restore the variable
          \ a0  = address of a variable
          \ x   = the current contents of _a0_
          \ a1  = return address
  r> swap              \ save return address _a1_
  dup @ 2>r            \ save variable address and value
  [ dup ] literal >r   \ force exit via _a_
  >r ;                 \ restore return address _a1_
  compile-only

  drop \ discard _a_

  \ doc{
  \
  \ local ( a -- )
  \
  \ Save the value of variable _a_, which will be restored at
  \ the end of the current definition.
  \
  \ ``local`` is a `compile-only` word.
  \
  \ Usage example:

  \ ----
  \ variable v
  \ 1 v !  v ?  \ default value
  \
  \ : test ( -- )
  \   v local
  \   v ?  1887 v !  v ? ;
  \
  \ v ?  \ default value
  \ ----
  \
  \ See: `arguments`, `anon`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-11-14: Adapted from the original code.
  \
  \ 2016-03-24: An alternative implementation with `:noname`.
  \
  \ 2016-04-24: Add `need :noname`, because `:noname` has been
  \ moved from the kernel to the library.
  \
  \ 2016-05-17: Need `>body`, which has been moved to the
  \ library.
  \
  \ 2016-12-30: Remove the old, unused, first version.
  \
  \ 2017-03-18: Improve the code: `:noname` and `>body` are not
  \ needed anymore.
  \
  \ The previous version used the following memory:

  \   Including requirements:
  \     Data space:  70 B
  \     Name space:  32 B
  \     Total:      102 B
  \   Not including requirements:
  \     Data space:  34 B
  \     Name space:  10 B
  \     Total:       44 B

  \ The new version uses the following memory:

  \     Data space: 31 B
  \     Name space: 10 B
  \     Total:      41 B

  \ Improve documentation.
  \
  \ 2017-05-22: Improve documentation.

  \ vim: filetype=soloforth
