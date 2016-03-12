/*

ld linker script

This file is part of Solo Forth
http://programandala.net/en.program.solo_forth.html

Copyright (C) 2015 Marcos Cruz (programandala.net)

Copying and distribution of this file, with or without
modification, are permitted in any medium without royalty
provided the copyright notice, the aknowledgments file and
this notice are preserved.  This file is offered as-is,
without any warranty.

2015-08-17: Start.

*/

MEMORY 
{

  /*
   * 0x5E00 is the address Solo Forth is loaded into and executed.
   * 0xA200 is the space to the top of memory.
   */

    forth : ORIGIN = 0x5E00, LENGTH = 0xA200
}

SECTIONS 
{

  /*
   * The text section contains the code and the dictionary
   * (except name fields and link fields).
   */

      .text : { *(.text) } >forth

  /*
   * The data section contains the name fields, the link fields,
   * and pointers to the code fields in the dictionary.  It is
   * assembled at 0xC000, but moved to a memory bank the first
   * time Solo Forth is executed.
   *
   * XXX FIXME this does not work, the data section is put after
   * the text section, not at 0xC000. But the syntax is right
   * according to the info manual.
      . = 0xC000;

      no difference with the ofsset:
      . = 0x6200;
   */

      . = 0xC000;
      .data : { *(.data) } >forth
      .bss : { *(.bss) } >forth

}
