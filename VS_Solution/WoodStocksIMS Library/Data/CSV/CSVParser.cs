using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Woodstocks.WoodstocksIMS.Data.CSV
{    
    /// <summary>
    /// A CSV Parser that converts a csv string into a <see cref="List{T}"/>.
    /// </summary>
    public class CSVParser
    {
        /// <summary>
        /// The character that is used to separate values in a csv value string. By default it is a comma (,).
        /// </summary>
        private char csvSeparator = ',';

        /// <summary>
        /// The character that is used to escape values in a csv value string. By default it is the double quotation (") character.
        /// </summary>
        private char escapeCharacter = '"';

        /// <summary>
        /// An enumeration that defines the values of trimming options.
        /// </summary>
        public enum TrimOption: uint {

            /// <summary>
            /// Used to indicate no trimming should occur.
            /// </summary>
            None = 0, 

            /// <summary>
            /// Used to indicate that only leading white space should be trimmed.
            /// </summary>
            LeadingWhitespace = 1, 

            /// <summary>
            /// Used to indicate that only trailing white space should be trimmed.
            /// </summary>
            TrailiingWhitespace = 2, 

            /// <summary>
            /// Used to indicate that both leading and trailing white space should be trimmed.
            /// </summary>
            LeadingAndTrailingWhitespace = 3 
        }

        /// <summary>
        /// A value used as a token in tokenizing a csv string.
        /// </summary>
        private string csvToken = "csvValue";

        /// <summary>
        /// A trim option that specifies whether the parser should remove whitespace from values 
        /// contained in a csv value string. By default the parser removes both leading and trailing 
        /// white space from any value within a csv value string.
        /// </summary>
        private TrimOption trimOption = TrimOption.LeadingAndTrailingWhitespace;

        /// <summary>
        /// Parses a csv string into a <see cref="List{T}"/> of strings that contain
        /// the component values of the csv string.
        /// </summary>
        /// <param name="input">The csv string that should be parsed into 
        /// a string List.</param>
        /// <param name="removeEscapeCharacter">Indicates whether escape characters (i.e
        /// double quotes should be removed from escaped values within the csv input string.</param>
        /// <returns></returns>
        /// <remarks>This method parses a csv string into a List that contains
        /// the individual values of a the string.
        /// 
        /// To ensure correct parsing of a csv value string, including those with escaped values,
        /// the string is tokenized prior to splitting it into its component values.
        /// 
        /// Once the string is tokenized it is split at the character specified by csvSeparator. 
        /// 
        /// Once split into its component values, each tokenized component is replaced restoring 
        /// the component value to its pre-tokenized value. 
        /// </remarks>
        public List<string> Parse(string input, bool removeEscapeCharacter)
        {
            Queue<string> replaced;
            string tokenizedString = Tokenize(input, out replaced);
            List<string> csvValues = new List<string>(tokenizedString.Split(csvSeparator));
            StripWhitespace(csvValues, trimOption);
            if ((replaced.Count > 0) && (removeEscapeCharacter))
            {
                replaced = TrimEscapeCharacter(replaced);
            }
            return Detokenize(csvValues, replaced);
        }

        /// <summary>
        /// Removes the escape character from a set of values from a csv value string.
        /// </summary>
        /// <param name="escapedValues">The individual values in the csv value string.</param>
        /// <returns>The value set with any escape caharacter removed.</returns>        
        protected virtual Queue<string> TrimEscapeCharacter(Queue<string> escapedValues)
        {
            Queue<string> unescapedValues = new Queue<string>();
            foreach (string escapedValue in escapedValues)
            {
                string s = escapedValue.Replace(escapeCharacter, ' ');
                s = s.Trim();
                unescapedValues.Enqueue(s);
            }
            return unescapedValues;
        }

        /// <summary>
        /// Replaces an escaped csv value with a csv token.
        /// </summary>
        /// <param name="csv"></param>
        /// <param name="replaced"></param>
        /// <returns></returns>
        /// <remarks>This method is used to replace any escaped csv values that are 
        /// delimited with double quotes with a csv token.</remarks>
        //A Queue is used to ensure that the order that the values are tokenized
        //is maintained, so that they can be detokenized correctly.
        virtual protected string Tokenize(string csv, out Queue<string> replaced)
        {
            replaced = new Queue<string>();
            Regex searchPattern = new Regex("\"[^\"]*\"");
            Match match = null;
            string tokenizedString = csv;
            while ( (match = searchPattern.Match(tokenizedString)).Success)
            {
                replaced.Enqueue(match.Value);
                tokenizedString = searchPattern.Replace(tokenizedString, csvToken, 1);
            }

            return tokenizedString;       
        }

        /// <summary>
        /// Removes the white space of values contained in values according to a 
        /// specified trimming option.
        /// </summary>
        /// <param name="values">The values that are to have white space removed.</param>
        /// <param name="trimOption">The trimming option that specifies how
        /// trimming is to occur. <see cref="TrimOption"/></param>
        virtual protected void StripWhitespace(List<string> values, TrimOption trimOption)
        {            
            for (int i = 0; i < values.Count; i++)
            {
                switch (trimOption)
                {
                    case TrimOption.LeadingWhitespace:
                        values[i] = values[i].TrimStart();
                        break;
                    case TrimOption.TrailiingWhitespace:
                        values[i] = values[i].TrimEnd();
                        break;
                    case TrimOption.LeadingAndTrailingWhitespace:
                        values[i] = values[i].Trim();
                        break;
                    case TrimOption.None:
                        values[i] = values[i];
                        break;
                    default:
                        values[i] = values[i].Trim();
                        break;                        
                }                
            }
        }

        /// <summary>
        /// Gets the number of tokenised values within List{T} of decomposed csv values. 
        /// </summary>
        /// <param name="tokenizedValues">The decomposed set of csv values.</param>
        /// <returns>The number of tokenised values contained within the List{T}.</returns>
        virtual protected int GetTokenCount(List<string> tokenizedValues)
        {
            int tokenCount = 0;
            foreach (string value in tokenizedValues)
            {
                if (value == csvToken)
                {
                    ++tokenCount;
                }
            }
            return tokenCount;
        }

        /// <summary>
        /// Detokenises a set of decomposed csv values.
        /// </summary>
        /// <param name="tokenizedValues">The decomposed List{T} of csv values.</param>
        /// <param name="replacements">The values to replace each csv token.</param>
        /// <returns>The detokenized set of csv values.</returns>
        /// <remarks>This method replaces each csv tokenized value with the original value
        /// before tokenization.</remarks>
        virtual protected List<string> Detokenize(List<string> tokenizedValues, Queue<string> replacements)
        {            
            if (replacements.Count != this.GetTokenCount(tokenizedValues))
                throw new Exception();
            List<string> detokenizedValues = new List<string>();
            for (int i = 0; i < tokenizedValues.Count; i++)
            {
                if (tokenizedValues[i] == csvToken)
                {
                    detokenizedValues.Add(replacements.Dequeue());                    
                }
                else
                {
                    detokenizedValues.Add(tokenizedValues[i]);
                }
            }
            return detokenizedValues;
        }
    }
}
