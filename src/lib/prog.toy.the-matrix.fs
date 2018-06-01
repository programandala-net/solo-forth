  \ prog.toy.the-matrix.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201806012147
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ A The Matrix display effect.

  \ ===========================================================
  \ Credit

  \ Adapted by Marcos Cruz (programandala.net), 2018,
  \ from code posted here:

  \ Newsgroups: comp.lang.forth
  \ Date: Tue, 29 May 2018 21:11:31 -0700 (PDT)
  \ Message-ID: <bc6f7262-5fa3-46ec-94c6-8491cd354754@googlegroups.com>
  \ Subject: (The) Matrix
  \ From: dxforth at gmail dot com

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ ===========================================================

( the-matrix )

need random need green need black need set-ink need set-paper
need set-bright need columns need rows

: the-matrix ( -- )
  green set-ink  black set-paper  black border  page
  begin
    2 random negate set-bright
    columns random rows random at-xy
    2 random '0' + emit
    key?
  until  key drop  default-colors ;

  \ ===========================================================
  \ Change log

  \ 2018-06-01: First version.

  \ vim: filetype=soloforth
