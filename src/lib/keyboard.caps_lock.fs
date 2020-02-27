  \ keyboard.casp_lock.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202002271822
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words related to manipulate caps lock.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018,
  \ 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( capslock )

need os-flags2 need ctoggle need cset need creset

%1000 os-flags2 2constant capslock ( -- b ca )

  \ doc{
  \
  \ capslock ( -- b ca )
  \
  \ Return address _ca_ of system variable FLAGS2 and bitmask
  \ _b_ of the bit that controls the status of capslock.
  \
  \ See: `set-capslock`, `unset-capslock`, `capslock?`,
  \ `os-flags2`.
  \
  \ }doc

: toggle-capslock ( -- ) capslock ctoggle ;

  \ doc{
  \
  \ toggle-capslock ( -- )
  \
  \ Toggle capslock.
  \
  \ See: `set-capslock`, `unset-capslock`, `capslock?`,
  \ `capslock`, `ctoggle`.
  \
  \ }doc

: set-capslock ( -- ) capslock cset ;

  \ doc{
  \
  \ set-capslock ( -- )
  \
  \ Set capslock.
  \
  \ See: `unset-capslock`, `capslock?`, `toggle-capslock`,
  \ `capslock`, `cset`.
  \
  \ }doc

: unset-capslock ( -- ) capslock creset ;

  \ doc{
  \
  \ unset-capslock ( -- )
  \
  \ Unset capslock.
  \
  \ See: `set-capslock`, `capslock?`, `toggle-capslock`,
  \ `capslock`, `creset`.
  \
  \ }doc

: capslock? ( -- f ) capslock c@and? ;

  \ doc{
  \
  \ capslock? ( -- f )
  \
  \ Is capslock set?
  \
  \ See: `set-capslock`, `unset-capslock`, `toggle-capslock`,
  \ `capslock`, `c@and?`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-11-23: Rename `c!toggle-bits` to `ctoggle`,
  \ `c!set-bits` to `cset`, `c!reset-bits` to `creset` and
  \ `c@test-bits` to `c@and`, after the changes in the system.
  \ Use `c@and?`.
  \
  \ 2017-01-10: Document all words.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2018-04-13: Improve documentation.
  \
  \ 2020-02-27: Fix typo in documentation.

  \ vim: filetype=soloforth
