﻿using System;
using System.Collections.Generic;

namespace PDFPatcher.Model
{
	[System.Diagnostics.DebuggerDisplay("{Direction}({Region.Top},{Region.Left})Lines={Lines.Count}")]
	sealed class TextRegion
	{
		internal Bound Region { get; }

		/// <summary>
		/// 获取文本区域中的行。
		/// 不应该调用此属性的 Add 方法添加行，而应使用 <see cref="AddLine"/> 方法。
		/// </summary>
		internal List<TextLine> Lines { get; } = [];
		internal WritingDirection Direction { get; set; }

		internal TextRegion(TextLine text) {
			Region = new Bound(text.Region);
			AddLine(text);
		}

		internal void AddLine(TextLine line) {
			if (Direction == WritingDirection.Unknown) {
				var d = Region.GetDistance(line.Region, WritingDirection.Unknown);
				Direction = (d.Location == DistanceInfo.Placement.Up || d.Location == DistanceInfo.Placement.Down)
					? WritingDirection.Vertical
					: (d.Location == DistanceInfo.Placement.Left || d.Location == DistanceInfo.Placement.Right)
					? WritingDirection.Horizontal
					: WritingDirection.Unknown;
			}
			Lines.Add(line);
			Region.Merge(line.Region);
		}


	}
}
