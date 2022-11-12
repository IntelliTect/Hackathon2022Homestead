using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homestead.Shared;
public class StartGameDto
{
    public int PlayerId { get; set; }
    public Game Game { get; set; } = null!;
}