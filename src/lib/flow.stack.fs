  \ flow.stack.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702220020

  \ -----------------------------------------------------------
  \ Description

  \ Words to manipulate the control flow stack.

  \ -----------------------------------------------------------
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ Latest changes

  \ 2017-01-19: Make `cs-pick`, `cs-roll`, `cs-swap` and
  \ `cs-drop` individually accessible to `need`.  Remove
  \ alternative unfinished implementations, ported from
  \ DX-Forth and hForth.

( cs-pick cs-roll cs-swap cs-drop )

[unneeded] cs-pick
?\ need alias need pick ' pick alias cs-pick

[unneeded] cs-roll
?\ need alias need roll ' roll alias cs-roll

[unneeded] cs-swap
?\ need alias ' swap alias cs-swap

[unneeded] cs-drop
?\ need alias ' drop alias cs-drop

  \ vim: filetype=soloforth
