  \ strings.replaces.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202007282031
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Forth-2012's `replaces`.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ ===========================================================
  \ Credit

  \ Code adapted from the Forth-2012 documentation.

( substitute-wordlist (substitution slit-substitution )

unneeding substitute-wordlist
?\  wordlist constant substitute-wordlist

  \ doc{
  \
  \ substitute-wordlist ( -- wid )
  \
  \ Word list for substitution names and replacement texts.
  \
  \ See also: `replaces`.
  \
  \ }doc

unneeding (substitution ?(

need substitute-wordlist need nextname

: (substitution ( ca1 len1 -- ca2 )
  get-current >r substitute-wordlist set-current
  nextname create here
  r> set-current ; ?)

  \ doc{
  \
  \ (substitution ( ca1 len1 -- ca2 ) "paren-substitution"
  \
  \ Given a string _ca1 len1_ create its definition in
  \ `substitute-wordlist` its substitution and return the
  \ address of its storage space in data space, not allocated.
  \
  \ ``(substitution`` is a common factor of `substitution` and
  \ `xt-substitution`.
  \
  \ See also: `substitution`, `xt-substitution`, `replaces`.
  \
  \ }doc

unneeding slit-substitution ?(

need (substitution need /counted-string

: slit-substitution ( ca1 len1 -- ca2 )
  (substitution 0 c, /counted-string chars allot
  does> count ; ?)

  \ doc{
  \
  \ substitution ( ca1 len1 -- ca2 )
  \
  \ Given a string _ca1 len1_ create its substitution and
  \ storage space.  Return the address of the buffer for the
  \ substitution text.
  \
  \ See also: `replaces`.
  \
  \ }doc

( find-substitution reuse-substitution replaces )

unneeding find-substitution ?(

need substitute-wordlist need search-wordlist

: find-substitution ( ca len -- xt true | false )
  substitute-wordlist search-wordlist ; ?)

  \ doc{
  \
  \ find-substitution ( ca len -- xt f | 0 )
  \
  \ Given a string _ca len_, find its substitution.  Return
  \ _xt_ and _f_ if found, or just zero if not found.
  \
  \ See also: `replaces`.
  \
  \ }doc

unneeding reuse-substitution ?(

need >body

: reuse-substitution ( ca len xt -- dfa ) nip nip >body ; ?)

unneeding replaces ?(

need find-substitution need reuse-substitution
need slit-substitution

: replaces ( ca1 len1 ca2 len2 -- )
  2dup find-substitution if    reuse-substitution
                         else  slit-substitution
                         then  place ; ?)

  \ doc{
  \
  \ replaces ( ca1 len1 ca2 len2 -- )
  \
  \ Set the string _ca1 len1_ as the text to substitute for the
  \ substitution named by _ca2 len2_. If the substitution does
  \ not exist it is created. The  program may then reuse the
  \ buffer _ca1 len1_ without affecting the definition  of the
  \ substitution.
  \
  \ The name of a substitution should not contain the "%"
  \ delimiter character.
  \
  \ ``replaces`` allots data space and creates a definition.
  \
  \ Origin: Forth-2012 (STRING EXT).
  \
  \ See also: `substitute`, `unescape`, `substitution`,
  \ `find-substitution`, `substitute-wordlist`, `replace`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-05-18: Use `wordlist` instead of `vocabulary`, which
  \ has been moved to the library.
  \
  \ 2016-11-26: Need `search-wordlist`, which has been moved to
  \ the library.
  \
  \ 2017-01-21: Finish. Test. Document. Make all words
  \ individually accessible to `need`.
  \
  \ 2017-01-22: Improve documentation.
  \
  \ 2017-01-23: Modify to support the new alternative
  \ `xt-replaces`:  Now execution of a substitution returns the
  \ string pair, not the address of a counted string.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-09-09: Update notation "pfa" to the standard "dfa".
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-09: Add words' pronunciaton.
  \
  \ 2020-05-04: Update documentation.

  \ vim: filetype=soloforth
