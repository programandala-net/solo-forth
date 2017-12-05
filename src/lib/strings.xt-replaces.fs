  \ strings.xt-replaces.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201709091154
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Al alternative to Forth-2012's `replaces`.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( xt-substitution xt-replaces )


[unneeded] xt-substitution ?(

need (substitution

: xt-substitution ( ca len -- a )
  (substitution  cell allot does> ( dfa ) perform ; ?)

  \ doc{
  \
  \ xt-substitution ( ca len -- a )
  \
  \ Given a string _ca len_ create its substitution and
  \ storage space.  Return the address that will hold the
  \ execution token of the substitution.
  \
  \ See: `xt-replaces`.
  \
  \ }doc

[unneeded] xt-replaces ?(

need find-substitution need reuse-substitution
need xt-substitution

: xt-replaces ( xt ca len -- )
  2dup find-substitution if    reuse-substitution
                         else  xt-substitution
                         then  ! ; ?)

  \ doc{
  \
  \ xt-replaces ( xt ca len -- )
  \
  \ Set _xt_ (whose execution returns the address and length of
  \ a string) as the text to substitute for the substitution
  \ named by _ca len_.  If the substitution does not exist it
  \ is created.
  \
  \ The name of a substitution should not contain the "%"
  \ delimiter character.
  \
  \ See: `replaces`, `substitute`, `unescape`,
  \ `substitute-wordlist`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2017-01-23: Start.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-09-09: Update notation "pfa" to the standard "dfa".

  \ vim: filetype=soloforth

