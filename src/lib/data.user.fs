  \ data.user.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201705111617
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words to use the user data space.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( ucreate ?user uallot user 2user )

[unneeded] ucreate ?\ : ucreate ( "name" -- ) udp @ (user) ;

  \ doc{
  \
  \ ucreate ( "name" -- )
  \
  \ Parse _name. Create a header _name_ which points to the
  \ first available offset within the user area.  When _name_
  \ is later executed, its absolute user area storage address
  \ is placed on the stack.  No user space is allocated.
  \
  \ See also: `uallot`, `user`, `2user`, `?user`.
  \
  \ }doc

[unneeded] ?user ?(

: ?user ( -- )
  udp @ dup /user > #-279 ?throw  0< #-280 ?throw ; ?)
  \ Error codes: #-279: user area overflow
  \              #-280: user area underflow

  \ doc{
  \
  \ ?user ( -- )
  \
  \ `throw` an exception if the user area pointer is out of
  \ bounds.
  \
  \ See also: `udp`, `/user`.
  \
  \ }doc

[unneeded] uallot ?( need ?user

: uallot ( n -- ) udp +! ?user ; ?)

  \ doc{
  \
  \ uallot ( n -- )
  \
  \ If _n_ is greater than zero, reserve _n_ address units of
  \ user data space. If _n_ is less than zero, release _n_
  \ address units of user data space. If _n_ is zero, leave the
  \ user data-space pointer unchanged. An exception is thrown
  \ if the user-data pointer is out of bounds after the
  \ operation.
  \
  \ See also: `udp`, `ucreate`, `?user`, `user`, `2user`.
  \
  \ }doc

[unneeded] user ?( need ucreate need uallot

: user ( "name" -- ) ucreate cell uallot ; ?)

  \ doc{
  \
  \ user ( n "name" -- )
  \
  \ Parse _name_. Create a user variable _name_ in the first
  \ available offset within the user area.
  \ When _name_ is
  \ later executed, its absolute user area storage address is
  \ placed on the stack.
  \
  \ See also: `2user`, `ucreate`, `uallot`, `?user`.
  \
  \ }doc

[unneeded] 2user ?( need ucreate need uallot

: 2user ( "name" -- )
  ucreate [ 2 cells ] literal uallot ; ?)

  \ doc{
  \
  \ 2user ( "name" -- )
  \
  \ Parse _name_. Create a user double variable _name_ in the
  \ first available offset within the user area.  When _name_
  \ is later executed, its absolute user area storage address
  \ is placed on the stack.
  \
  \ See also: `user`, `ucreate`, `uallot`, `?user`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-09: First draft.
  \
  \ 2016-04-21: New version, moved from the kernel.
  \
  \ 2016-11-18: Add conditional compilation. Improve
  \ documentation.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-19: Remove remaining `exit` at the end of
  \ conditional interpretation.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-05-11: Improve documentation.

  \ vim: filetype=soloforth
