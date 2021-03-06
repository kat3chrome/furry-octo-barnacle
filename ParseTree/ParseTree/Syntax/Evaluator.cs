﻿using System;

namespace ParseTree.Syntax
{
    internal class Evaluator
    {
        private readonly ExpressionSyntax _root;

        public Evaluator(ExpressionSyntax root)
        {
            this._root = root;
        }

        public int Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private int EvaluateExpression(SyntaxNode node)
        {
            while (true)
            {
                switch (node)
                {
                    case LiteralExpressionSyntax literalExpression:
                        return (int)literalExpression.LiteralToken.Value;
                    case UnaryExpressionSyntax unaryExpression:
                    {
                        var operand = EvaluateExpression(unaryExpression.Operand);

                        return unaryExpression.OperatorToken.Kind switch
                        {
                            SyntaxKind.PlusToken => operand,
                            SyntaxKind.MinusToken => -operand,
                            _ => throw new Exception($"Unexpected binary operator {unaryExpression.OperatorToken.Kind}")
                        };
                    }
                    case BinaryExpressionSyntax binaryExpression:
                    {
                        var left = EvaluateExpression(binaryExpression.Left);
                        var right = EvaluateExpression(binaryExpression.Right);

                        return binaryExpression.OperatorToken.Kind switch
                        {
                            SyntaxKind.PlusToken => left + right,
                            SyntaxKind.MinusToken => left - right,
                            SyntaxKind.StarToken => left * right,
                            SyntaxKind.SlashToken => left / right,
                            _ => throw new Exception($"Unexpected binary operator {binaryExpression.Kind}")
                        };
                    }
                    case ParenthesizedExpressionSyntax parenthesized:
                        node = parenthesized.Expression;
                        continue;
                    default:
                        throw new Exception($"Unexpected node {node.Kind}");
                }
            }
        }
    }
}