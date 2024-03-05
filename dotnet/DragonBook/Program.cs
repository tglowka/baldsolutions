// See https://aka.ms/new-console-template for more information

using DragonBook.lexer;
using DragonBook.parser;

var program = """
              {
                  int i; int j; float v; float x; float[100] a;
                  while(true)
                  {
                      do i = i+1; while(a[i] < v);
                      do j = j-1; while(a[j] > v);
                      if(i>=j) break;
                      x = a[i];
                      a[i] = a[j];
                      a[j] = x;
                  }
              }
              """;

var lexer = new Lexer(program);
var parser = new Parser(lexer);
var result = parser.Parse();
Console.WriteLine(result);
